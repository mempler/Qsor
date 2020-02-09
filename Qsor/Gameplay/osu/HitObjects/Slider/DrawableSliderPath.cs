// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository https://github.com/ppy/osu for full licence text.
// https://github.com/ppy/osu/blob/726abaddc3282a84183224672799ed7812ecdb81/osu.Game.Rulesets.Osu/Objects/Drawables/Pieces/DrawableSliderPath.cs

using osu.Framework.Graphics.Lines;
using osuTK.Graphics;

namespace Qsor.Gameplay.osu.HitObjects.Slider
{
    public abstract class DrawableSliderPath : SmoothPath
    {
        protected const float BorderPortion = 0.128f;

        private const float BorderMaxSize = 8f;
        private const float BorderMinSize = 0f;

        private Color4 _borderColour = Color4.White;

        public Color4 BorderColour
        {
            get => _borderColour;
            set
            {
                if (_borderColour == value)
                    return;

                _borderColour = value;

                InvalidateTexture();
            }
        }

        private Color4 _accentColour = Color4.White;

        public Color4 AccentColour
        {
            get => _accentColour;
            set
            {
                if (_accentColour == value)
                    return;

                _accentColour = value;

                InvalidateTexture();
            }
        }

        private float _borderSize = 1;

        public float BorderSize
        {
            get => _borderSize;
            set
            {
                if (_borderSize == value)
                    return;

                if (value < BorderMinSize || value > BorderMaxSize)
                    return;

                _borderSize = value;

                InvalidateTexture();
            }
        }

        protected float CalculatedBorderPortion => BorderSize * BorderPortion;
    }
}