using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using Qsor.Overlays;

namespace Qsor.Screens
{
    public class MainMenuScreen : Screen
    {
        private Container ToolBarTop;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            ToolBarTop = new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 80
            };
            
            ToolBarTop.Add(new UserOverlay());
            
            AddInternal(ToolBarTop);
        }
    }
}