using osu.Framework.Graphics.Containers;

namespace Qsor.Game.Gameplay
{
    public abstract class HitObject : CompositeDrawable
    {
        public abstract float BeginTime { get; }
        public abstract float EndTime { get; }

        public virtual float Duration => EndTime - BeginTime;
    }
}