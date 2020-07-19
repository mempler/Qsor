using System.IO;
using System.Threading.Tasks;
using osu.Framework.IO.Network;

namespace Qsor.Game.Online
{
    public class BeatmapMirrorAccess
    {
        private const string BeatmapMirror = "https://storage.ripple.moe";
        
        public static async Task<Stream> DownloadBeatmap(int setId)
        {
            var wr = new WebRequest(BeatmapMirror + $"/d/{setId}");
            
            await wr.PerformAsync();

            return wr.ResponseStream;
        }
    }
}