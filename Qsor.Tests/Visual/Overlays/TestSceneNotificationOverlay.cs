using System;
using System.Collections.Generic;
using NLipsum.Core;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Framework.Testing;
using osuTK.Graphics;
using Qsor.Game.Overlays;
using Qsor.Game.Overlays.Drawables;
using Qsor.Game.Overlays.Notifications;

namespace Qsor.Tests.Visual.Overlays
{
    public class TestSceneNotificationOverlay : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes
            => new[] {typeof(NotificationOverlay), typeof(DrawableNotification)};

        [BackgroundDependencyLoader]
        private void Load()
        {
            var lipsumGenerator = new LipsumGenerator();
            var notificationOverlay = new NotificationOverlay();
            
            AddSetupStep("Setup Overlay", () =>
            {
                Add(notificationOverlay = new NotificationOverlay());
            });

            AddStep("Create random Lorem Ipsum", () =>
            {
                var random = new Random();
                var randomColour = new Color4((float) random.NextDouble(), (float) random.NextDouble(), (float) random.NextDouble(), 1f);

                notificationOverlay.AddNotification(
                    new LocalisedString(lipsumGenerator.GenerateLipsum(4, Features.Sentences, FormatStrings.Paragraph.LineBreaks)),
                    randomColour, 5000);
            });
        }
    }
}