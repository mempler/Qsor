using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsGameplayCategory : ISettingsCategory
    {
        public string Name => "Gameplay";
        public IconUsage Icon => FontAwesome.Regular.Circle;
    }
}