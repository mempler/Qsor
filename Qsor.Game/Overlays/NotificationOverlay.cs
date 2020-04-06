using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osuTK;
using Qsor.Game.Overlays.Notifications;

namespace Qsor.Game.Overlays
{
    public class NotificationOverlay : CompositeDrawable
    {
        private FillFlowContainer<DrawableNotification> _drawableNotifications;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;

            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;

            _drawableNotifications = new FillFlowContainer<DrawableNotification>
            {
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.X,
                RelativeSizeAxes = Axes.Y,
                Spacing = new Vector2(6),
                LayoutEasing = Easing.OutQuint,
                LayoutDuration = 400
            };

            Padding = new MarginPadding(10);

            AddInternal(_drawableNotifications);
        }

        public void AddNotification(LocalisedString text, ColourInfo colourInfo, int duration = -1, Action clickAction = null)
            => AddNotification(new DrawableNotification(text, colourInfo, duration, clickAction));

        public void AddNotification(DrawableNotification notification)
        {
            Scheduler.AddOnce(() =>
            {
                notification.Anchor = Anchor.BottomRight;
                notification.Origin = Anchor.BottomRight;
                notification.Alpha = 0;
                
                _drawableNotifications.Add(notification);
                _drawableNotifications.SetLayoutPosition(notification, -_drawableNotifications.FlowingChildren.Count());
            
                notification.FadeInFromZero(200);
            });
        }
    }
}