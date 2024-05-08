using osu.Framework.Allocation;
using osu.Framework.Testing;
using Qsor.Game.Graphics.UserInterface.Overlays;

namespace Qsor.Tests.Visual.Overlays
{
    public partial class TestSceneUserOverlay : TestScene
    {
         [BackgroundDependencyLoader]
         private void Load()
         {
             AddStep("Setup", () => Add(new UserOverlay()));
         }
    }
}