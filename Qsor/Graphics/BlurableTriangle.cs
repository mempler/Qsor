using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using Qsor.Containers;

namespace Qsor.Graphics
{
    //TODO: Finish
    public class BlurredTriangle : CompositeDrawable
    {
        /// <summary>
        /// Blur a Triangle in a cool way!
        /// </summary>
        /// <param name="backBuffer">Background for the Blur as example <see cref="BackgroundImageContainer"/></param>
        /// <param name="blur"></param>
        public BlurredTriangle(BufferedContainer backBuffer, float blur)
        {
            Size = new Vector2(200);
            Masking = true;
            
            CornerRadius = 20;
            BorderColour = Color4.Magenta;
            BorderThickness = 2;
 
            InternalChildren = new Drawable[]
            {
                new Triangle
                {
                    //RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(50),
                    Anchor = Anchor.Centre,
                    Colour = Color4.Red
                },
                
                new BufferedContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    BlurSigma = new Vector2(blur),
                    Children = new Drawable[]
                    {
                        backBuffer
                            .CreateView()
                            .With(d =>
                            {
                                d.RelativeSizeAxes = Axes.Both;
                                d.SynchronisedDrawQuad = true;
                                d.DisplayOriginalEffects = true;
                            }),
                    },
                }
            };
        }
    }
}