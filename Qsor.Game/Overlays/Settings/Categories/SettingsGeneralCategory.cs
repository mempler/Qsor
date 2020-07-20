using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using Qsor.Game.Overlays.Settings.Drawables;
using Qsor.Game.Overlays.Settings.Drawables.Objects;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsGeneralCategory : SettingsCategoryContainer
    {
        public override string Name => "General";
        public override IconUsage Icon => FontAwesome.Solid.Cog;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            var subcat = new DrawableSettingsSubCategory("Something");
            subcat.Content.Add(new DrawableSettingsCheckbox(true, "Some label", "Some tooltip"));
            subcat.Content.Add(new DrawableSettingsCheckbox(false, "Some label 2", "Some tooltip 2"));
            subcat.Content.Add(new DrawableSettingsCheckbox(true, "Some label 3", "Some tooltip 3"));
            
            AddInternal(subcat);
        }
    }
}