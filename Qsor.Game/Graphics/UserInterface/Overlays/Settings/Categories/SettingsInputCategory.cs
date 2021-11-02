using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Categories
{
    public class SettingsInputCategory : SettingsCategoryContainer
    {
        public override string Name => "Input";
        public override IconUsage Icon => FontAwesome.Solid.Gamepad;
    }
}