using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Testing;
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
        private void Load()
        {
            var settingsOverlay = new SettingsOverlay();

            AddSetupStep("Setup Overlay", () =>
            {
                if (settingsOverlay != null)
                    Remove(settingsOverlay);
                
                Add(settingsOverlay = new SettingsOverlay());
            });

            AddStep("Show Overlay", settingsOverlay.Show);
            AddStep("Hide Overlay", settingsOverlay.Hide);
        }
    }
}