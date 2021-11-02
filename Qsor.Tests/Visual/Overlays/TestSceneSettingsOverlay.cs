using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;
using Qsor.Game.Graphics.Containers;
using Qsor.Game.Graphics.UserInterface.Overlays;

namespace Qsor.Tests.Visual.Overlays
{
    public class TestSceneSettingsOverlay : TestScene
    {
        [BackgroundDependencyLoader]
        private void Load(TextureStore ts)
        {
            var bg = new BackgroundImageContainer();
            Add(bg);
            bg.SetTexture(ts.Get("https://3.bp.blogspot.com/-906HDJiF4Nk/UbAN4_DrK_I/AAAAAAAAAvE/pZQwo-u2RbQ/s1600/Background+images.jpg"));
            
            SettingsOverlay settingsOverlay;
            Add(settingsOverlay = new SettingsOverlay());
            
            AddStep("Show Overlay", settingsOverlay.Show);
            AddStep("Hide Overlay", settingsOverlay.Hide);
        }
    }
}