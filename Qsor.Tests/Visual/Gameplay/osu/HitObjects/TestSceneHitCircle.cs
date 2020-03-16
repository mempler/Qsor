using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Framework.Timing;
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
                var clock = new StopwatchClock();
                
                var hc = new HitCircle(beatmap, Vector2.Zero)
                {
                    BeginTime = 600,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    
                    HitObjectColour = Color4.Red,
                    
                    Clock = new FramedClock(clock)
                };
                Add(hc);
                
                clock.Start();
                
                hc.Show();
            });
        }
        
        
    }
}
