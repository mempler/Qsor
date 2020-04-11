using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
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
        }
    }
}