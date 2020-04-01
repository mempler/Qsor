using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using Qsor.Overlays;

namespace Qsor.Tests.Visual.Overlays
{
    public class TestSceneQsorLogoOverlay : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes
            => new List<Type>{ typeof(QsorLogoOverlay) };

        private Track _track;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audioManager)
        {            
            _track = audioManager.Tracks.Get("WATEVA - Ber Zer Ker (Rob Gasser Remix) [NCS Release]");

            AddStep("Setup", () =>
            {
                Child = new QsorLogoOverlay
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Track = _track
                };
            });
            
            AddToggleStep("Music", e =>
            {
                if (e)
                    _track.Start();
                else
                    _track.Stop();
            });
            
            AddSliderStep("Track Volume", 0, 100, 20, d => _track.Volume.Value = d / 100f);
            AddSliderStep("Track Frequency", 5, 200, 100, d => _track.Frequency.Value = d / 100f);
            AddSliderStep("Track Tempo", 5, 100, 100, d => _track.Tempo.Value = d / 100f);
        }
    }
}