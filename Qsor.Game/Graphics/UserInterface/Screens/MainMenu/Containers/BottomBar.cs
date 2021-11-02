using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace Qsor.Game.Graphics.UserInterface.Screens.MainMenu.Containers
{
    public class BottomBar : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            FillMode = FillMode.Fit;
            Anchor = Anchor.BottomLeft;
            Origin = Anchor.BottomLeft;
            Height = 80;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    RelativeSizeAxes =  Axes.Both, 
                    Colour = Color4.Black,
                    Alpha = .4f
                }
            };
        }
    }
}