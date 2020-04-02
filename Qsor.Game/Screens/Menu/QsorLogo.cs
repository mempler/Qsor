// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using Qsor.Game.Beatmaps;
using Qsor.Game.Graphics.Containers;

namespace Qsor.Game.Screens.Menu
{
    public class QsorLogo : BeatSyncedContainer
    {
        private const double EarlyActivation = 60;

        private Container _logoHoverContainer;
        private Container _logoBeatContainer;
        private Sprite _qsorLogo;
        private Sprite _ripple;
        private Sprite _ghostingLogo;
        
        private LogoVisualisation _visualisation;
        private int _lastBeatIndex;
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore ts)
        {
            AddInternal(new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                
                Children = new []
                {
                    _logoHoverContainer = new CircularContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        
                        Children = new Drawable[]
                        {
                            _logoBeatContainer = new Container
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                AutoSizeAxes = Axes.Both,
                                
                                Children = new Drawable[]
                                {
                                    _ripple = new Sprite
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Texture = ts.Get("Logo-ghost"),
                                    },
                                    _qsorLogo = new Sprite
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Texture = ts.Get("Logo"),
                                    },
                                    _ghostingLogo = new Sprite
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Texture = ts.Get("Logo-ghost"),
                                        Alpha = .2f
                                    },
                                    _visualisation = new LogoVisualisation
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre
                                    },
                                }
                            }
                        },
                    }
                }
            });
            
            CornerRadius = Math.Min(_qsorLogo.Width, _qsorLogo.Height) / 2f;
        }
        
        protected override void Update()
        {
            base.Update();

            const float scaleAdjustCutoff = 0.4f;

            if (Beatmap.Value?.Track?.IsRunning == true)
            {
                var maxAmplitude = _lastBeatIndex >= 0 ? Beatmap.Value.Track.CurrentAmplitudes.Maximum : 0;
                
                _qsorLogo.ScaleTo(1 - Math.Max(0, maxAmplitude - scaleAdjustCutoff) * 0.04f, 75, Easing.OutQuint);
                _ghostingLogo.ScaleTo(1 + Math.Max(0, maxAmplitude - scaleAdjustCutoff) * 0.04f, 75, Easing.OutQuint);
            }
            
            _visualisation.Scale = _qsorLogo.Scale;
            _visualisation.Size = _qsorLogo.Size;
        }

        protected override void OnNewBeat(int beatIndex, TimingPoint timingPoint, TrackAmplitudes amplitudes)
        {
            _lastBeatIndex = beatIndex;
            
            var amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

            _logoBeatContainer
                .ScaleTo(1 - 0.05f * amplitudeAdjust, EarlyActivation, Easing.Out)
                .Then()
                .ScaleTo(1, timingPoint.MsPerBeat * 2, Easing.OutQuint);
            
            _ghostingLogo
                .ScaleTo(1 + 0.05f * amplitudeAdjust, EarlyActivation, Easing.Out)
                .Then()
                .ScaleTo(1, timingPoint.MsPerBeat * 2, Easing.OutQuint);
            
            _ripple.ClearTransforms();
            _ripple
                .ScaleTo(_qsorLogo.Scale)
                .ScaleTo(_qsorLogo.Scale * (1 + 0.16f * amplitudeAdjust), timingPoint.MsPerBeat, Easing.OutQuint)
                .FadeTo(0.3f * amplitudeAdjust).FadeOut(timingPoint.MsPerBeat, Easing.OutQuint);

            if (timingPoint.KiaiMode)
            {
                _visualisation.ClearTransforms();
                _visualisation
                    .FadeTo(0.9f * amplitudeAdjust, EarlyActivation, Easing.Out)
                    .Then()
                    .FadeTo(0.5f, timingPoint.MsPerBeat);
            }
        }
        
        protected override bool OnHover(HoverEvent e)
        {
            _logoHoverContainer.ScaleTo(1.05f, 100, Easing.In);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            _logoHoverContainer.ScaleTo(1, 100, Easing.Out);
        }
    }
}