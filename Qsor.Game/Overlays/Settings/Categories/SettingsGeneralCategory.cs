using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsGeneralCategory : ISettingsCategory
    {
        public string Name => "General";
        public IconUsage Icon => FontAwesome.Solid.Cog;
    }
}