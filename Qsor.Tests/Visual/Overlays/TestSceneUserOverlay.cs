using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using Qsor.Game.Online.Users;
using Qsor.Game.Online.Users.Drawables;
using Qsor.Game.Overlays;
using Qsor.Game.Overlays.Drawables;

namespace Qsor.Tests.Visual.Overlays
{
    public class TestSceneUserOverlay : TestScene
    {
         [BackgroundDependencyLoader]
         private void Load()
         {
             AddStep("Setup", () => Add(new UserOverlay()));
         }
    }
}