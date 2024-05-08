using osu.Framework.Allocation;
using osu.Framework.Testing;
using Qsor.Game;

namespace Qsor.Tests.Visual
{
    public partial class TestSceneQsorGame : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            AddGame(new QsorGame(new []{ string.Empty }));
        }
    }
}