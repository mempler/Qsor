using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osuTK;

namespace Qsor.Gameplay.osu.Containers
{
    public class PlayfieldContainer : Container
    {
        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }

        private static readonly Vector2 PlayfieldSize = new Vector2(512, 384);
        public override Vector2 Size { get => PlayfieldSize; set {} }

        private double currentTime;
        protected override void Update()
        {
            if (BeatmapManager.Song?.IsRunning == false) // Improve performance by not even Updating the HitObjects.
                return;

            currentTime = BeatmapManager.Song?.CurrentTime + BeatmapManager.ActiveBeatmap.General.AudioLeadIn ?? 0;
            
            Children // It's faster to iterate through Children. (or should be as there are less objects)
                .Select(obj => obj as HitObject)
                .Where(obj => obj != null)
                .Where(obj => currentTime > obj.EndTime)
                .ForEach(obj =>
                {
                    obj.Hide();

                    //Remove(obj);
                });


            BeatmapManager.ActiveBeatmap.HitObjects
                .Where(obj => currentTime < obj.EndTime)
                .Where(obj => currentTime > obj.BeginTime - (BeatmapManager.ActiveBeatmap.Difficulty.ApproachRate + 200))
                .Where(obj => !Children.Contains(obj))
                .ForEach(obj =>
                {
                    Add(obj);
                    
                    obj.Show();
                });
        }
    }
}