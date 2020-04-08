using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Qsor.Game.Gameplay.Containers
{
    public abstract class Playfield : Container
    {
        public Playfield()
        {
            RelativeSizeAxes = Axes.Both;
        }
    }
}