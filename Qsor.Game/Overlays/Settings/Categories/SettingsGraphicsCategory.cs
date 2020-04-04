using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using Qsor.Game.Overlays.Settings.Drawables;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsGraphicsCategory : SettingsCategoryContainer
    {
        public override string Name => "Graphics";
        public override IconUsage Icon => FontAwesome.Solid.Desktop;

        [BackgroundDependencyLoader]
        private void Load()
        {
            var dropdown = new DrawableSettingsDropdown();
            dropdown.Add(new DropdownItem("1", "1"));
            dropdown.Add(new DropdownItem("2", "2"));
            dropdown.Add(new DropdownItem("3", "3"));
            dropdown.Add(new DropdownItem("4", "4"));
            Add(dropdown);
        }
    }
}