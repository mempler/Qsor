using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsOnlineCategory : ISettingsCategory
    {
        public string Name => "Online";
        public IconUsage Icon => FontAwesome.Solid.GlobeAmericas;
    }
}