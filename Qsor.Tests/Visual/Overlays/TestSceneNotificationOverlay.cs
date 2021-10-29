using System;
using System.Collections.Generic;
using NLipsum.Core;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Framework.Testing;
using osuTK.Graphics;
using Qsor.Game.Overlays;
using Qsor.Game.Overlays.Notifications;

namespace Qsor.Tests.Visual.Overlays
{
    public class TestSceneNotificationOverlay : TestScene
    {
        private LipsumGenerator _lipsumGenerator;
        private NotificationOverlay _notificationOverlay;

        [SetUpSteps]
        public void Setup()
        {
            // Background
            Add(new Box
            {
                Colour = new Color4(0.2f, 0.2f, 0.2f, 1.0f),
                RelativeSizeAxes = Axes.Both,
            });
            
            _lipsumGenerator = new LipsumGenerator();
            _notificationOverlay = new NotificationOverlay();
            
            Add(_notificationOverlay = new NotificationOverlay());
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            AddStep("Create random Lorem Ipsum", () =>
            {
                var random = new Random();
                var randomColour = new Color4((float) random.NextDouble(), (float) random.NextDouble(), (float) random.NextDouble(), 1f);

                _notificationOverlay.AddNotification(
                    new LocalisableString(_lipsumGenerator.GenerateLipsum(4, Features.Sentences, FormatStrings.Paragraph.LineBreaks)
                        .Trim()),
                    randomColour, 5000);
            });

            AddStep("Create Big Notification", () =>
            {
                _notificationOverlay.AddBigNotification(
                    new LocalisableString(_lipsumGenerator.GenerateLipsum(4, Features.Sentences, FormatStrings.Paragraph.LineBreaks)
                        .Trim()),
                    5000);
            });
        }
    }
}