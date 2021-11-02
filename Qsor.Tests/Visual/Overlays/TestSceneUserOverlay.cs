using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using Qsor.Game.Graphics.UserInterface.Overlays;
using Qsor.Game.Online.Users;

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