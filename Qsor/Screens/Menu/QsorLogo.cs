using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using Qsor.Beatmaps;
using Qsor.Graphics.Containers;
using Qsor.Screens.Menu;

namespace Qsor.Overlays
{
    public class QsorLogoOverlay : BeatSyncedContainer
    {
        private Sprite _qsorLogo;
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore ts)
        {
            AddInternal(_qsorLogo = new Sprite
            {
                Texture = ts.Get("Logo"),
                Origin = Anchor.Centre,
            });

            AddInternal(new LogoVisualisation
            {
                Origin = Anchor.Centre,
                Size = _qsorLogo.Size,
            });
        }
        
        protected override void Update()
        {
            base.Update();

            if (Beatmap.Value.Track.CurrentAmplitudes.Average * 1.1f < 1)
                return;

            _qsorLogo.
                ScaleTo(Math.Clamp(Beatmap.Value.Track.CurrentAmplitudes.Average * 1.1f, 1.0f, 1.2f), 150)
                .Then(e => e.ScaleTo(1f, 100));
        }

        protected override void OnNewBeat(int beatIndex, TimingPoint timingPoint, TrackAmplitudes amplitudes)
        {
            _qsorLogo?.
                ScaleTo(1.025f, 100)
                .Then(e => e.ScaleTo(1f, 100));
        }
    }
}