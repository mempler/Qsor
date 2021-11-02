using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings
{
    public abstract class SettingsCategoryContainer : Container
    {
        public new abstract string Name { get; }
        public abstract IconUsage Icon { get; }

        public SettingsCategoryContainer()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
        }
    }
}