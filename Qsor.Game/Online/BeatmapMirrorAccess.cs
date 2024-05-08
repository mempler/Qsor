using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.IO.Network;
using osu.Framework.Platform;

namespace Qsor.Game.Online
{
    public partial class BeatmapMirrorAccess : IDependencyInjectionCandidate
    {
        private const string BeatmapMirror = "https://api.nerinyan.moe";

        [Resolved]
        private Storage Storage { get; set; }
        
        /// <summary>
        /// Download a beatmap from a given <see cref="BeatmapMirror"/>
        /// </summary>
        /// <param name="setId">Set ID</param>
        public void DownloadBeatmap(int setId)
        {
            using var fileReq = new FileWebRequest(Storage.GetFullPath($"Songs/{setId}.osz"), $"{BeatmapMirror}/d/{setId}");
            
            fileReq.Perform(); // Works
        }
        
        /// <summary>
        /// Download a beatmap from a given <see cref="BeatmapMirror"/> asynchronously
        /// </summary>
        /// <param name="setId">Set ID</param>
        /// 
        /// NOTE: this seems broken right now.
        /// 
        public async Task DownloadBeatmapAsync(int setId)
        {
            using var fileReq = new FileWebRequest(Storage.GetFullPath($"Songs/{setId}.osz"), $"{BeatmapMirror}/d/{setId}");
            
            await fileReq.PerformAsync(); // Gets stuck
        }
    }
}