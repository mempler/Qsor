using osu.Framework.Screens;
using Qsor.Game.Gameplay.Containers;
using Qsor.Game.Gameplay.Rulesets.osu.Containers;
using Qsor.Game.Gameplay.Rulesets.osu.Screens;

namespace Qsor.Game.Gameplay.Rulesets.osu
{
    public class OsuRuleset : Ruleset
    {
        public override int Id => 0;
        public override string Name => "Circle"; // We can't use osu for copyright reasons. TODO: find a better name than just Circle
        
        public override Playfield CreatePlayfield() => new OsuPlayfield();
        public override Screen CreateScreen() => new OsuScreen();
    }
}