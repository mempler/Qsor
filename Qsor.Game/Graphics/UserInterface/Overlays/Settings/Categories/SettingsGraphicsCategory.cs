using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Categories
{
    public class SettingsGraphicsCategory : SettingsCategoryContainer
    {
        public override string Name => "Graphics";
        public override IconUsage Icon => FontAwesome.Solid.Desktop;

        [BackgroundDependencyLoader]
        private void Load()
        {

        }
    }
}