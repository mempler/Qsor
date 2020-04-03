using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsInputCategory : ISettingsCategory
    {
        public string Name => "Input";
        public IconUsage Icon => FontAwesome.Solid.Gamepad;
    }
}