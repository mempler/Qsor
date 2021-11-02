using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK.Graphics;
using Qsor.Game.Graphics.UserInterface.Overlays;

namespace Qsor.Tests.Visual.Overlays
{
    public class TestSceneMusicPlayerOverlay : TestScene
    {
        private MusicPlayerOverlay _musicPlayer;
        
        [SetUpSteps]
        public void Setup()
        {
            // Background
            Add(new Box
            {
                Colour = new Color4(0.2f, 0.2f, 0.2f, 1.0f),
                RelativeSizeAxes = Axes.Both,
            });
            
            Add(_musicPlayer = new MusicPlayerOverlay());
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
        }
    }
}