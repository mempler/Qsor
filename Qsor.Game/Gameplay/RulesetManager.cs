using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;

namespace Qsor.Game.Gameplay
{
    public class RulesetManager : Component
    {
        public IEnumerable<Ruleset> Rulesets { get; } = new List<Ruleset>();

        public Ruleset GetRuleset(int id) => Rulesets.FirstOrDefault(s => s.Id == id);
    }
}