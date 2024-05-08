using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using Qsor.Game.Graphics.UserInterface.Overlays;

namespace Qsor.Tests.Visual.Overlays
{
    public partial class TestSceneUpdaterOverlay : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            Console.WriteLine("Test");
            
            var updaterOverlay = new UpdaterOverlay();

            Add(updaterOverlay);

            AddStep("Show", updaterOverlay.Show);
            AddStep("Hide", updaterOverlay.Hide);
        }
        
    }
}