using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Platform;
using osu.Framework.Testing;
using osuTK.Graphics;
using Qsor.Game;

namespace Qsor.Tests.Visual
{
    public class TestSceneQsorGame : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            AddGame(new QsorGame(new []{ string.Empty }));
        }
    }
}