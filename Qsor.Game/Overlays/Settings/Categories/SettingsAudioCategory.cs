using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsAudioCategory : ISettingsCategory
    {
        public string Name => "Audio";
        public IconUsage Icon => FontAwesome.Solid.VolumeUp;
    }
}