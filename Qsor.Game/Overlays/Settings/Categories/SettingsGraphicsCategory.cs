using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using Qsor.Game.Overlays.Settings.Drawables.Objects;

namespace Qsor.Game.Overlays.Settings.Categories
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