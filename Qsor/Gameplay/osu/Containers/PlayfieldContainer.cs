using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osuTK;
using Qsor.Containers.Input;

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
        
        private double _currentTime;
        protected override void Update()
        {
            if (BeatmapManager.ActiveBeatmap.Track?.IsRunning == false) // Improve performance by not even Updating the HitObjects.
                return;
            
            _currentTime = BeatmapManager.ActiveBeatmap.Track?.CurrentTime + BeatmapManager.ActiveBeatmap.General.AudioLeadIn ?? 0;
            
            Children // It's faster to iterate through Children. (or should be as there are less objects)
                .OfType<HitObject>() // TODO: remove
                .Where(obj => _currentTime > obj.EndTime)
                .ForEach(obj =>
                {
                    obj.Hide();

                    //Remove(obj);
                });

            BeatmapManager.ActiveBeatmap.HitObjects
                .Where(obj => _currentTime < obj.EndTime)
                .Where(obj => _currentTime > obj.BeginTime - (BeatmapManager.ActiveBeatmap.Difficulty.ApproachRate + 600))
                .Where(obj => !Children.Contains(obj))
                .ForEach(obj =>
                {
                    Add(obj);
                    
                    obj.Show();
                });
        }
    }
}