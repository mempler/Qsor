using osu.Framework.Screens;
using Qsor.Game.Gameplay.Containers;

namespace Qsor.Game.Gameplay
{
    public abstract class Ruleset
    {
        public abstract int Id { get; }
        public abstract string Name { get; }

        public abstract Playfield CreatePlayfield();
        public abstract Screen CreateScreen();
    }
}