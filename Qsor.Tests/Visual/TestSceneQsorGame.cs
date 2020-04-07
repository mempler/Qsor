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
        private void Load(GameHost host)
        {
            var game = new QsorGame(new []{ string.Empty });
            game.SetHost(host);

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                },
                game
            };

            AddUntilStep("Wait for Load", () => game.IsLoaded);
        }
    }
}