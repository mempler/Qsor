using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using OsuParsers.Replays;
using osuTK;
using Qsor.Containers.Input;
using Qsor.Gameplay.osu.HitObjects;

namespace Qsor.Gameplay.osu.Containers
{
    public class PlayfieldContainer : Container
    {
        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }

        private static readonly Vector2 PlayfieldSize = new Vector2(512, 384);
        public override Vector2 Size { get => PlayfieldSize; set {} }

        public VirtualCursorContainer Cursor { get; private set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddInternal(Cursor = new VirtualCursorContainer {Depth = -int.MaxValue}); // always draw cursor ontop of everything
        }
        
        private double currentTime;
        protected override void Update()
        {
            if (BeatmapManager.Song?.IsRunning == false) // Improve performance by not even Updating the HitObjects.
                return;
            
            Scale = new Vector2((Parent.DrawHeight / PlayfieldSize.Y) / 2); // Proper scaling
            
            currentTime = BeatmapManager.Song?.CurrentTime + BeatmapManager.ActiveBeatmap.General.AudioLeadIn ?? 0;
            
            Children // It's faster to iterate through Children. (or should be as there are less objects)
                .OfType<HitObject>()
                .Where(obj => currentTime > obj.EndTime)
                .ForEach(obj =>
                {
                    obj.Hide();

                    //Remove(obj);
                });

            BeatmapManager.ActiveBeatmap.HitObjects
                .Where(obj => currentTime < obj.EndTime)
                .Where(obj => currentTime > obj.BeginTime - (BeatmapManager.ActiveBeatmap.Difficulty.ApproachRate + 600))
                .Where(obj => !Children.Contains(obj))
                .ForEach(obj =>
                {
                    Add(obj);
                    
                    obj.Show();
                });
        }
    }
}