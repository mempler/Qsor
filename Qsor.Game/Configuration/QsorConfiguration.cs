using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Qsor.Game.Configuration
{
    public class QsorConfigManager : IniConfigManager<QsorSetting>
    {
        protected override void InitialiseDefaults()
        {
            Set(QsorSetting.BeatmapSetId, 756794);
            Set(QsorSetting.BeatmapFile, "TheFatRat - Mayday (feat. Laura Brehm) (Voltaeyx) [[2B] Calling Out Mayday].osu");
        }

        public QsorConfigManager(Storage storage) :
            base(storage)
        {
        }
    }

    public enum QsorSetting
    {
        BeatmapFile,
        BeatmapSetId
    }
}