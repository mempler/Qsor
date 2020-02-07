using System.Drawing;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using Qsor.Containers;
using Qsor.osu;
using Qsor.osu.HitObjects;

namespace Qsor.Screens
{
    public class BeatmapScreen : Screen
    {
        [Resolved]
        private AudioManager audioManager { get; set; }
        
        
        public PlayfieldContainer Playfield;

        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }
        
        protected override void LoadComplete()
        {
            BeatmapManager.LoadBeatmap("./songs/1/TheFatRat - Mayday (feat. Laura Brehm) (Voltaeyx) [[2B] Calling Out Mayday].osu");
            BeatmapManager.PlayBeatmap();
            
            base.LoadComplete();
        }
        
        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(2000, Easing.OutQuint);
        }
    }
}