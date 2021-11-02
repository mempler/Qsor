// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using Qsor.Game.Beatmaps;
using Qsor.Game.Graphics.Containers;

namespace Qsor.Game.Graphics.UserInterface.Screens.MainMenu.Drawables
{
    public class DrawableMenuSideFlashes : BeatSyncedContainer
    {
        private readonly IBindable<WorkingBeatmap> _beatmap = new Bindable<WorkingBeatmap>();

        private Box _leftBox;
        private Box _rightBox;

        private const float AmplitudeDeadZone = 0.25f;
        private const float AlphaMultiplier = 1.0f;
        private const float KiaiMultiplier = 1.0f;

        private const int BoxMaxAlpha = 200;
        private const double BoxFadeInTime = 65;
        private const int BoxWidth = 200;
        
        public DrawableMenuSideFlashes()
        {
            EarlyActivationMilliseconds = BoxFadeInTime;

            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void Load(BeatmapManager beatmapManager)
        {
            _beatmap.BindTo(beatmapManager.WorkingBeatmap);
            
            Children = new Drawable[]
            {
                _leftBox = new Box
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Y,
                    Width = BoxWidth * 2,
                    Height = 1.5f,
                    // align off-screen to make sure our edges don't become visible during parallax.
                    X = -BoxWidth,
                    Alpha = 0,
                    Blending = BlendingParameters.Additive
                },
                _rightBox = new Box
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    RelativeSizeAxes = Axes.Y,
                    Width = BoxWidth * 2,
                    Height = 1.5f,
                    X = BoxWidth,
                    Alpha = 0,
                    Blending = BlendingParameters.Additive
                }
            };
            
            var baseColour = Color4.White;
            var gradientDark = baseColour.Opacity(0).ToLinear();
            var gradientLight = baseColour.Opacity(0.6f).ToLinear();

            _leftBox.Colour = ColourInfo.GradientHorizontal(gradientLight, gradientDark);
            _rightBox.Colour = ColourInfo.GradientHorizontal(gradientDark, gradientLight);
        }

        private int _customBeatIndex = 0;
        protected override void OnNewBeat(int beatIndex, TimingPoint timingPoint, ChannelAmplitudes amplitudes)
        {
            var meter = timingPoint.Parent?.Meter ?? timingPoint.Meter;
            if ((beatIndex < 0 || meter < 0) && !timingPoint.KiaiMode)
                return;

            if (timingPoint.KiaiMode)
            {
                _customBeatIndex += 1;
            }
            
            if (timingPoint.KiaiMode ? _customBeatIndex % 2 == 0 : beatIndex % meter == 0)
                Flash(_leftBox, timingPoint.MsPerBeat, timingPoint.KiaiMode, amplitudes);
            if (timingPoint.KiaiMode ? _customBeatIndex % 2 == 1 : beatIndex % meter == 0)
                Flash(_rightBox, timingPoint.MsPerBeat, timingPoint.KiaiMode, amplitudes);
        }

        private void Flash(Drawable d, double beatLength, bool kiai, ChannelAmplitudes amplitudes)
        {
            d.FadeTo(kiai ? KiaiMultiplier : AlphaMultiplier, BoxFadeInTime)
             .Then()
             .FadeOut(beatLength, Easing.In);
        }
    }
}
