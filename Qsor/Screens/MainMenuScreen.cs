using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using Qsor.Containers;
using Qsor.Overlays;

namespace Qsor.Screens
{
    public class MainMenuScreen : Screen
    {
        private Container _toolBarTop;
        private BackgroundImageContainer _background;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            _toolBarTop = new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 80
            };
            
            _toolBarTop.Add(new UserOverlay());
            
            AddInternal(_background = new BackgroundImageContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
            
            AddInternal(_toolBarTop);
        }
    }
}