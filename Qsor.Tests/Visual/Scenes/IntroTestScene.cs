using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Testing;
using Qsor.Game.Graphics.UserInterface.Screens;

namespace Qsor.Tests.Visual.Scenes
{
    [TestFixture]
    public class IntroTestScene : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            var stack = new ScreenStack();
            
            Add(stack);
    
            AddStep("Start sequence", () =>
            {
                stack.Push(new IntroScreen());
            });
            
            AddStep("Exit sequence", () =>
            {
                stack.Exit();
            });
        }
    }
}