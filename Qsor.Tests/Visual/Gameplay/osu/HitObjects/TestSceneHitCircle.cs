using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using Qsor.Gameplay.osu;
using Qsor.Gameplay.osu.HitObjects;

namespace Qsor.Tests.Visual.Gameplay.osu.HitObjects
{
    public class TestSceneHitCircle : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(HitCircle) };

        [BackgroundDependencyLoader]
        private void Load()
        {
            var beatmap = new Beatmap();
            
            
            AddStep("Spawn HitCircle", () =>
            {
                var hc = new HitCircle(beatmap, Vector2.Zero)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    
                    HitObjectColour = Color4.Red
                };



                Add(hc);
                
                hc.Show();
            });
        }
        
        
    }
}
