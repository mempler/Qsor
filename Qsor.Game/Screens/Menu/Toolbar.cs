using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using Qsor.Game.Overlays;

namespace Qsor.Game.Screens.Menu
{
    public class Toolbar : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            FillMode = FillMode.Fit;
            Height = 80;
            
            AddInternal(new Box
            {
                Name = "Background",
                RelativeSizeAxes =  Axes.Both, 
                Colour = Color4.Black,
                Alpha = .4f
            });
            
            AddInternal(new UserOverlay());
            
            AddInternal(new MusicPlayerOverlay
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            });
        }
    }
}