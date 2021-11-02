// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Utils;
using osuTK;

namespace Qsor.Game.Graphics.Containers
{
    public class ParallaxContainer : Container, IRequireHighFrequencyMousePosition
    {
        public const float DefaultParallaxAmount = 0.02f;

        /// <summary>
        /// The amount of parallax movement. Negative values will reverse the direction of parallax relative to user input.
        /// </summary>
        public float ParallaxAmount = DefaultParallaxAmount;
        public ParallaxContainer()
        {
            RelativeSizeAxes = Axes.Both;
            
            AddInternal(_content = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }

        public readonly Container _content;
        private InputManager _input;

        protected override Container<Drawable> Content => _content;

        [BackgroundDependencyLoader]
        private void Load()
        {
            _content.MoveTo(Vector2.Zero, _firstUpdate ? 0 : 1000, Easing.OutQuint);
            _content.Scale = new Vector2(1 + Math.Abs(ParallaxAmount));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            _input = GetContainingInputManager();
        }

        private bool _firstUpdate = true;

        protected override void Update()
        {
            base.Update();

            var offset = (_input.CurrentState.Mouse == null ? Vector2.Zero : ToLocalSpace(_input.CurrentState.Mouse.Position) - DrawSize / 2) * ParallaxAmount;

            const float parallaxDuration = 100;

            var elapsed = Math.Clamp(Clock.ElapsedFrameTime, 0, parallaxDuration);

            _content.Position = Interpolation.ValueAt(elapsed, _content.Position, offset, 0, parallaxDuration, Easing.OutQuint);
            _content.Scale = Interpolation.ValueAt(elapsed, _content.Scale, new Vector2(1 + Math.Abs(ParallaxAmount)), 0, 1000, Easing.OutQuint);

            _firstUpdate = false;
        }
    }
}