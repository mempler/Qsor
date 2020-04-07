using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Testing;
using Qsor.Game.Screens;
using Qsor.Game.Screens.Menu;

namespace Qsor.Tests.Visual.Scenes
{
    [TestFixture]
    public class MainMenuTestScene : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(MainMenuScreen)
        };

        [BackgroundDependencyLoader]
        private void Load()
        {
            var stack = new ScreenStack();
            
            Add(stack);
    
            AddStep("Start sequence", () =>
            {
                stack.Push(new MainMenuScreen());
            });
            
            AddStep("Exit sequence", () =>
            {
                stack.Exit();
            });
        }
    }
}