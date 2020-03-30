// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;

namespace Qsor.Gameplay.osu.HitObjects.Slider
{
    public interface ISliderProgress
    {
        /// <summary>
        /// Updates the progress of this <see cref="ISliderProgress"/> element along the slider.
        /// </summary>
        /// <param name="completionProgress">Amount of the slider completed.</param>
        void UpdateProgress(double completionProgress);
    }
    
    /// <summary>
    /// A <see cref="SliderBody"/> which changes its curve depending on the snaking progress.
    /// </summary>
    public class SnakingSliderBody : SliderBody, ISliderProgress
    {
        public readonly List<Vector2> CurrentCurve = new List<Vector2>();

        public readonly Bindable<bool> SnakingIn = new Bindable<bool>();
        public readonly Bindable<bool> SnakingOut = new Bindable<bool>();

        public double? SnakedStart { get; private set; }
        public double? SnakedEnd { get; private set; }

        public override float PathRadius
        {
            get => base.PathRadius;
            set
            {
                if (base.PathRadius == value)
                    return;

                base.PathRadius = value;

                Refresh();
            }
        }

        public override Vector2 PathOffset => _snakedPathOffset;

        /// <summary>
        /// The top-left position of the path when fully snaked.
        /// </summary>
        private Vector2 _snakedPosition;

        /// <summary>
        /// The offset of the path from <see cref="_snakedPosition"/> when fully snaked.
        /// </summary>
        private Vector2 _snakedPathOffset;

        private readonly HitSlider _slider;

        public SnakingSliderBody(HitSlider slider)
        {
            _slider = slider;
        }
        
        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Refresh();
        }

        public void UpdateProgress(double completionProgress)
        {
            var span = _slider.SpanAt(completionProgress);
            var spanProgress = _slider.ProgressAt(completionProgress);

            double start = 0;
            var end = SnakingIn.Value ? Math.Clamp(((BeatmapManager.ActiveBeatmap.Track.CurrentTime - (_slider.BeginTime - _slider.EndTime)) / (_slider.EndTime / 3)), 0, 1) : 1;

            if (span >= _slider.SpanCount() - 1)
            {
                if (Math.Min(span, _slider.SpanCount() - 1) % 2 == 1)
                {
                    start = 0;
                    end = SnakingOut.Value ? spanProgress : 1;
                }
                else
                {
                    start = SnakingOut.Value ? spanProgress : 0;
                }
            }

            SetRange(start, end);
        }

        public void Refresh()
        {
            // Generate the entire curve
            _slider.Path.GetPathToProgress(CurrentCurve, 0, 1);
            SetVertices(CurrentCurve);

            // Force the body to be the final path size to avoid excessive autosize computations
            Path.AutoSizeAxes = Axes.Both;
            Size = Path.Size;

            UpdatePathSize();

            _snakedPosition = Path.PositionInBoundingBox(Vector2.Zero);
            _snakedPathOffset = Path.PositionInBoundingBox(Path.Vertices[0]);

            var lastSnakedStart = SnakedStart ?? 0;
            var lastSnakedEnd = SnakedEnd ?? 0;

            SnakedStart = null;
            SnakedEnd = null;

            SetRange(lastSnakedStart, lastSnakedEnd);
        }

        public override void RecyclePath()
        {
            base.RecyclePath();
            UpdatePathSize();
        }

        private void UpdatePathSize()
        {
            // Force the path to its final size to avoid excessive framebuffer resizes
            Path.AutoSizeAxes = Axes.None;
            Path.Size = Size;
        }

        private void SetRange(double p0, double p1)
        {
            if (p0 > p1)
                (p0, p1) = (p1, p0);

            if (SnakedStart == p0 && SnakedEnd == p1) return;

            SnakedStart = p0;
            SnakedEnd = p1;

            _slider.Path.GetPathToProgress(CurrentCurve, p0, p1);

            SetVertices(CurrentCurve);

            // The bounding box of the path expands as it snakes, which in turn shifts the position of the path.
            // Depending on the direction of expansion, it may appear as if the path is expanding towards the position of the slider
            // rather than expanding out from the position of the slider.
            // To remove this effect, the path's position is shifted towards its final snaked position
            
            Path.Position = _snakedPosition - Path.PositionInBoundingBox(Vector2.Zero);
        }
    }
}
