using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using Qsor.Game.Beatmaps;
using Qsor.Game.Gameplay.osu.HitObjects;
using Qsor.Game.Gameplay.osu.HitObjects.Slider;

namespace Qsor.Tests.Visual.Gameplay.osu.HitObjects
{
    public class TestSceneHitSlider : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            var beatmap = new Beatmap();
            
            AddStep("Spawn HitSlider", () =>
            {
                var clock = new StopwatchClock();
                
                var hs = new HitSlider(beatmap, PathType.Linear, new []{ new Vector2(200, 200), new Vector2(400, 200) }, 250, 2)
                {
                    BeginTime = 600,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    
                    HitObjectColour = Color4.Red,
                    
                    Clock = new FramedClock(clock)
                };
                Add(hs);
                
                clock.Start();
                
                hs.Show();
            });
        }
    }
}
