using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;
using Qsor.Game.Graphics.Containers;
using Qsor.Game.Overlays;
using Qsor.Game.Overlays.Settings.Categories;
using Qsor.Game.Overlays.Settings.Drawables;

namespace Qsor.Tests.Visual.Overlays
{
    public class TestSceneSettingsOverlay : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes
            => new[]
            {
                typeof(SettingsOverlay), typeof(SettingsGeneralCategory),
                typeof(DrawableSettingsToolBar), typeof(DrawableSettingsIconSprite),
                typeof(DrawableSettingsMenu), typeof(DrawableSettingsCategory)
            };

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