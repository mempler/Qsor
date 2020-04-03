using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsGraphicsCategory : ISettingsCategory
    {
        public string Name => "Graphics";
        public IconUsage Icon => FontAwesome.Solid.Desktop;
    }
}