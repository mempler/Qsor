using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsSkinCategory : ISettingsCategory
    {
        public string Name => "Skin";
        public IconUsage Icon => FontAwesome.Solid.PaintBrush;
    }
}