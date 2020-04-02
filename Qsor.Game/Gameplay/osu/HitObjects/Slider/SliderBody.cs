// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository https://github.com/ppy/osu for full licence text.
// https://github.com/ppy/osu/blob/726abaddc3282a84183224672799ed7812ecdb81/osu.Game.Rulesets.Osu/Objects/Drawables/Pieces/DrawableSliderPath.cs

using System;
using System.Collections.Generic;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Gameplay.osu.HitObjects.Slider
{
    public abstract class SliderBody : CompositeDrawable
    {
        private DrawableSliderPath _path;

        protected Path Path => _path;

        public virtual float PathRadius
        {
            get => _path.PathRadius;
            set => _path.PathRadius = value;
        }

        /// <summary>
        /// Offset in absolute coordinates from the start of the curve.
        /// </summary>
        public virtual Vector2 PathOffset => _path.PositionInBoundingBox(_path.Vertices[0]);

        /// <summary>
        /// Used to colour the path.
        /// </summary>
        public Color4 AccentColour
        {
            get => _path.AccentColour;
            set
            {
                if (_path.AccentColour == value)
                    return;

                _path.AccentColour = value;
            }
        }

        /// <summary>
        /// Used to colour the path border.
        /// </summary>
        public new Color4 BorderColour
        {
            get => _path.BorderColour;
            set
            {
                if (_path.BorderColour == value)
                    return;

                _path.BorderColour = value;
            }
        }

        /// <summary>
        /// Used to size the path border.
        /// </summary>
        public float BorderSize
        {
            get => _path.BorderSize;
            set
            {
                if (_path.BorderSize == value)
                    return;

                _path.BorderSize = value;
            }
        }

        protected SliderBody()
        {
            RecyclePath();
        }

        /// <summary>
        /// Initialises a new <see cref="DrawableSliderPath"/>, releasing all resources retained by the old one.
        /// </summary>
        public virtual void RecyclePath()
        {
            InternalChild = _path = CreateSliderPath().With(p =>
            {
                p.Position = _path?.Position ?? Vector2.Zero;
                p.PathRadius = _path?.PathRadius ?? 10;
                p.AccentColour = _path?.AccentColour ?? Color4.White;
                p.BorderColour = _path?.BorderColour ?? Color4.White;
                p.BorderSize = _path?.BorderSize ?? 1;
                p.Vertices = _path?.Vertices ?? Array.Empty<Vector2>();
            });
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => _path.ReceivePositionalInputAt(screenSpacePos);

        /// <summary>
        /// Sets the vertices of the path which should be drawn by this <see cref="SliderBody"/>.
        /// </summary>
        /// <param name="vertices">The vertices</param>
        protected void SetVertices(IReadOnlyList<Vector2> vertices) => _path.Vertices = vertices;

        protected virtual DrawableSliderPath CreateSliderPath() => new DefaultDrawableSliderPath();

        private class DefaultDrawableSliderPath : DrawableSliderPath
        {
            private const float ShadowPortion = 0.06f;

            private new Color4 AccentColour => new Color4(base.AccentColour.R, base.AccentColour.G, base.AccentColour.B, base.AccentColour.A * 0.70f);

            protected override Color4 ColourAt(float position)
            {
                var realBorderPortion = ShadowPortion + CalculatedBorderPortion;
                var realGradientPortion = 1 - realBorderPortion;

                if (position <= ShadowPortion)
                    return new Color4(0f, 0f, 0f, 0.25f * position / ShadowPortion);

                if (position <= realBorderPortion)
                    return BorderColour;

                position -= realBorderPortion;

                var outerColour = AccentColour.Darken(0.1f);
                var innerColour = lighten(AccentColour, 0.5f);

                return Interpolation.ValueAt(position / realGradientPortion, outerColour, innerColour, 0, 1);
            }
            
            /// <summary>
            /// Lightens a colour in a way more friendly to dark or strong colours.
            /// </summary>
            private static Color4 lighten(Color4 color, float amount)
            {
                amount *= 0.5f;
                return new Color4(
                    Math.Min(1, color.R * (1 + 0.5f * amount) + 1 * amount),
                    Math.Min(1, color.G * (1 + 0.5f * amount) + 1 * amount),
                    Math.Min(1, color.B * (1 + 0.5f * amount) + 1 * amount),
                    color.A);
            }
        }
    }
}
