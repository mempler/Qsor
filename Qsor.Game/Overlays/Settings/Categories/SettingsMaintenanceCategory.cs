using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsMaintenanceCategory : ISettingsCategory
    {
        public string Name => "Maintenance";
        public IconUsage Icon => FontAwesome.Solid.Wrench;
    }
}