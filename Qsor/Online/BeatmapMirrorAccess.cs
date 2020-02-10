using System.IO;
using System.Threading.Tasks;
using osu.Framework.IO.Network;

namespace Qsor.Online
{
    public class BeatmapMirrorAccess
    {
        public const string BeatmapMirror = "https://pisstau.be";
        
        public async Task<Stream> DownloadBeatmap(uint setId)
        {
            var wr = new WebRequest(BeatmapMirror + $"/d/{setId}");
            
            await wr.PerformAsync();

            return wr.ResponseStream;
        }
    }
}