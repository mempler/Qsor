// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository https://github.com/ppy/osu for full licence text.
// https://github.com/ppy/osu/blob/master/osu.Game/Rulesets/Objects/SliderPath.cs
// https://github.com/ppy/osu/blob/master/osu.Game/Rulesets/Objects/Types/PathType.cs
// https://github.com/ppy/osu/blob/master/osu.Game/Rulesets/Objects/Types/IHasDistance.cs
// https://github.com/ppy/osu/blob/master/osu.Game/Rulesets/Objects/Types/IHasEndTime.cs
// https://github.com/ppy/osu/blob/master/osu.Game/Rulesets/Objects/Types/IHasRepeats.cs

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Utils;
using osuTK;

namespace Qsor.Gameplay.osu
{
    public enum PathType
    {
        Catmull,
        Bezier,
        Linear,
        PerfectCurve
    }
    
    public interface IHasCurve : IHasRepeats
    {
        /// <summary>
        /// The curve.
        /// </summary>
        SliderPath Path { get; }
    }
    
    /// <summary>
    /// A HitObject that ends at a different time than its start time.
    /// </summary>
    public interface IHasEndTime
    {
        /// <summary>
        /// The time at which the HitObject ends.
        /// </summary>
        double EndTime { get; }

        /// <summary>
        /// The duration of the HitObject.
        /// </summary>
        double Duration { get; }
    }
    
    public static class HasCurveExtensions
    {
        /// <summary>
        /// Computes the position on the curve relative to how much of the <see cref="HitObject"/> has been completed.
        /// </summary>
        /// <param name="obj">The curve.</param>
        /// <param name="progress">[0, 1] where 0 is the start time of the <see cref="HitObject"/> and 1 is the end time of the <see cref="HitObject"/>.</param>
        /// <returns>The position on the curve.</returns>
        public static Vector2 CurvePositionAt(this IHasCurve obj, double progress)
            => obj.Path.PositionAt(obj.ProgressAt(progress));

        /// <summary>
        /// Computes the progress along the curve relative to how much of the <see cref="HitObject"/> has been completed.
        /// </summary>
        /// <param name="obj">The curve.</param>
        /// <param name="progress">[0, 1] where 0 is the start time of the <see cref="HitObject"/> and 1 is the end time of the <see cref="HitObject"/>.</param>
        /// <returns>[0, 1] where 0 is the beginning of the curve and 1 is the end of the curve.</returns>
        public static double ProgressAt(this IHasCurve obj, double progress)
        {
            var p = progress * obj.SpanCount() % 1;
            if (obj.SpanAt(progress) % 2 == 1)
                p = 1 - p;
            return p;
        }

        /// <summary>
        /// Determines which span of the curve the progress point is on.
        /// </summary>
        /// <param name="obj">The curve.</param>
        /// <param name="progress">[0, 1] where 0 is the beginning of the curve and 1 is the end of the curve.</param>
        /// <returns>[0, SpanCount) where 0 is the first run.</returns>
        public static int SpanAt(this IHasCurve obj, double progress)
            => (int)(progress * obj.SpanCount());
    }
    
    /// <summary>
    /// A HitObject that spans some length.
    /// </summary>
    public interface IHasRepeats : IHasEndTime
    {
        /// <summary>
        /// The amount of times the HitObject repeats.
        /// </summary>
        int RepeatCount { get; set; }
    }

    public static class HasRepeatsExtensions
    {
        /// <summary>
        /// The amount of times the length of this <see cref="IHasRepeats"/> spans.
        /// </summary>
        /// <param name="obj">The object that has repeats.</param>
        public static int SpanCount(this IHasRepeats obj) => obj.RepeatCount + 1;
    }
    
    public struct SliderPath : IEquatable<SliderPath>
    {
        /// <summary>
        /// The user-set distance of the path. If non-null, <see cref="Distance"/> will match this value,
        /// and the path will be shortened/lengthened to match this length.
        /// </summary>
        public double? ExpectedDistance;

        /// <summary>
        /// The type of path.
        /// </summary>
        public readonly PathType Type;
        
        private Vector2[] _controlPoints;

        private List<Vector2> _calculatedPath;
        private List<double> _cumulativeLength;

        private bool _isInitialised;

        /// <summary>
        /// Creates a new <see cref="SliderPath"/>.
        /// </summary>
        /// <param name="type">The type of path.</param>
        /// <param name="controlPoints">The control points of the path.</param>
        /// <param name="expectedDistance">A user-set distance of the path that may be shorter or longer than the true distance between all
        /// <paramref name="controlPoints"/>. The path will be shortened/lengthened to match this length.
        /// If null, the path will use the true distance between all <paramref name="controlPoints"/>.</param>
        public SliderPath(PathType type, Vector2[] controlPoints, double? expectedDistance = null)
        {
            this = default;
            _controlPoints = controlPoints;

            Type = type;
            ExpectedDistance = expectedDistance;

            EnsureInitialised();
        }

        /// <summary>
        /// The control points of the path.
        /// </summary>
        public ReadOnlySpan<Vector2> ControlPoints
        {
            get
            {
                EnsureInitialised();
                return _controlPoints.AsSpan();
            }
        }

        /// <summary>
        /// The distance of the path after lengthening/shortening to account for <see cref="ExpectedDistance"/>.
        /// </summary>
        public double Distance
        {
            get
            {
                EnsureInitialised();
                return _cumulativeLength.Count == 0 ? 0 : _cumulativeLength[^1];
            }
        }

        /// <summary>
        /// Computes the slider path until a given progress that ranges from 0 (beginning of the slider)
        /// to 1 (end of the slider) and stores the generated path in the given list.
        /// </summary>
        /// <param name="path">The list to be filled with the computed path.</param>
        /// <param name="p0">Start progress. Ranges from 0 (beginning of the slider) to 1 (end of the slider).</param>
        /// <param name="p1">End progress. Ranges from 0 (beginning of the slider) to 1 (end of the slider).</param>
        public void GetPathToProgress(List<Vector2> path, double p0, double p1)
        {
            EnsureInitialised();

            var d0 = ProgressToDistance(p0);
            var d1 = ProgressToDistance(p1);

            path.Clear();

            var i = 0;

            for (; i < _calculatedPath.Count && _cumulativeLength[i] < d0; ++i)
            {
            }

            path.Add(InterpolateVertices(i, d0));

            for (; i < _calculatedPath.Count && _cumulativeLength[i] <= d1; ++i)
                path.Add(_calculatedPath[i]);

            path.Add(InterpolateVertices(i, d1));
        }

        /// <summary>
        /// Computes the position on the slider at a given progress that ranges from 0 (beginning of the path)
        /// to 1 (end of the path).
        /// </summary>
        /// <param name="progress">Ranges from 0 (beginning of the path) to 1 (end of the path).</param>
        /// <returns></returns>
        public Vector2 PositionAt(double progress)
        {
            EnsureInitialised();

            var d = ProgressToDistance(progress);
            return InterpolateVertices(IndexOfDistance(d), d);
        }

        private void EnsureInitialised()
        {
            if (_isInitialised)
                return;

            _isInitialised = true;

            _controlPoints = _controlPoints ?? Array.Empty<Vector2>();
            _calculatedPath = new List<Vector2>();
            _cumulativeLength = new List<double>();

            CalculatePath();
            CalculateCumulativeLength();
        }

        private IEnumerable<Vector2> CalculateSubpath(ReadOnlySpan<Vector2> subControlPoints)
        {
            switch (Type)
            {
                case PathType.Linear:
                    return PathApproximator.ApproximateLinear(subControlPoints);

                case PathType.PerfectCurve:
                    //we can only use CircularArc iff we have exactly three control points and no dissection.
                    if (ControlPoints.Length != 3 || subControlPoints.Length != 3)
                        break;

                    // Here we have exactly 3 control points. Attempt to fit a circular arc.
                    var subpath = PathApproximator.ApproximateCircularArc(subControlPoints);

                    // If for some reason a circular arc could not be fit to the 3 given points, fall back to a numerically stable bezier approximation.
                    if (subpath.Count == 0)
                        break;

                    return subpath;

                case PathType.Catmull:
                    return PathApproximator.ApproximateCatmull(subControlPoints);
            }

            return PathApproximator.ApproximateBezier(subControlPoints);
        }

        private void CalculatePath()
        {
            _calculatedPath.Clear();

            // Sliders may consist of various subpaths separated by two consecutive vertices
            // with the same position. The following loop parses these subpaths and computes
            // their shape independently, consecutively appending them to calculatedPath.

            var start = 0;
            var end = 0;

            for (var i = 0; i < ControlPoints.Length; ++i)
            {
                end++;

                if (i == ControlPoints.Length - 1 || ControlPoints[i] == ControlPoints[i + 1])
                {
                    var cpSpan = ControlPoints.Slice(start, end - start);

                    foreach (var t in CalculateSubpath(cpSpan))
                        if (_calculatedPath.Count == 0 || _calculatedPath.Last() != t)
                            _calculatedPath.Add(t);

                    start = end;
                }
            }
        }

        private void CalculateCumulativeLength()
        {
            double l = 0;

            _cumulativeLength.Clear();
            _cumulativeLength.Add(l);

            for (var i = 0; i < _calculatedPath.Count - 1; ++i)
            {
                var diff = _calculatedPath[i + 1] - _calculatedPath[i];
                double d = diff.Length;

                // Shorted slider paths that are too long compared to the expected distance
                if (ExpectedDistance.HasValue && ExpectedDistance - l < d)
                {
                    _calculatedPath[i + 1] = _calculatedPath[i] + diff * (float)((ExpectedDistance - l) / d);
                    _calculatedPath.RemoveRange(i + 2, _calculatedPath.Count - 2 - i);

                    l = ExpectedDistance.Value;
                    _cumulativeLength.Add(l);
                    break;
                }

                l += d;
                _cumulativeLength.Add(l);
            }

            // Lengthen slider paths that are too short compared to the expected distance
            if (ExpectedDistance.HasValue && l < ExpectedDistance && _calculatedPath.Count > 1)
            {
                var diff = _calculatedPath[^1] - _calculatedPath[^2];
                double d = diff.Length;

                if (d <= 0)
                    return;

                // ReSharper disable once UseIndexFromEndExpression (this breaks the Compiler)
                _calculatedPath[_calculatedPath.Count -1] += diff * (float)((ExpectedDistance - l) / d);
                _cumulativeLength[_calculatedPath.Count - 1] = ExpectedDistance.Value;
            }
        }

        private int IndexOfDistance(double d)
        {
            var i = _cumulativeLength.BinarySearch(d);
            if (i < 0) i = ~i;

            return i;
        }

        private double ProgressToDistance(double progress)
        {
            return Math.Clamp(progress, 0, 1) * Distance;
        }

        private Vector2 InterpolateVertices(int i, double d)
        {
            if (_calculatedPath.Count == 0)
                return Vector2.Zero;

            if (i <= 0)
                return _calculatedPath.First();
            if (i >= _calculatedPath.Count)
                return _calculatedPath.Last();

            var p0 = _calculatedPath[i - 1];
            var p1 = _calculatedPath[i];

            var d0 = _cumulativeLength[i - 1];
            var d1 = _cumulativeLength[i];

            // Avoid division by and almost-zero number in case two points are extremely close to each other.
            if (Precision.AlmostEquals(d0, d1))
                return p0;

            var w = (d - d0) / (d1 - d0);
            return p0 + (p1 - p0) * (float)w;
        }

        public bool Equals(SliderPath other)
        {
            if (ControlPoints == null && other.ControlPoints != null)
                return false;
            if (other.ControlPoints == null && ControlPoints != null)
                return false;

            return ControlPoints.SequenceEqual(other.ControlPoints) && ExpectedDistance.Equals(other.ExpectedDistance) && Type == other.Type;
        }
    }
}
