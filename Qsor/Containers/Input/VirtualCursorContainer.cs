using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Containers.Input
{
    // TODO: Fully Implement!
    public class VirtualCursorContainer : Container
    {
        private Box _cursor;


        [BackgroundDependencyLoader]
        private void Load()
        {
            _cursor = new Box
            {
                Size = new Vector2(64, 64),
                Colour = Color4.Red,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };
            
            AddInternal(_cursor);
        }
        

    }
}