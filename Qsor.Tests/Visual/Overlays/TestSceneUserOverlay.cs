using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osuTK;
using Qsor.Online.Users;
using Qsor.Online.Users.Drawables;
using Qsor.Overlays;
using Qsor.Overlays.Drawables;

namespace Qsor.Tests.Visual.Overlays
{
    public class TestSceneUserOverlay : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(UserOverlay),
            typeof(DrawableAvatar),
            typeof(DrawableLevelBar),
            typeof(UserStatistics),
        };

         [BackgroundDependencyLoader]
         private void Load()
         {
             AddStep("Setup", () => Add(new UserOverlay()));
         }
    }
}