using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Notifications
{
    public class DrawableBigNotification : CompositeDrawable
    {
        private readonly StopwatchClock _clock;
        private readonly double _duration;
        private readonly TextFlowContainer _textFlowContainer;
        private Box _background;

        public DrawableBigNotification(LocalisableString text, int duration = -1)
        {
            _clock = new StopwatchClock();
            _textFlowContainer = new TextFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                
                Direction = FillDirection.Full,
                AutoSizeAxes = Axes.Both,
                Padding = new MarginPadding(16),
                TextAnchor = Anchor.Centre // Centre text
            };
            
            _duration = duration == -1 ? Math.Max(3000, (double) (text.ToString().Length * 100)) : duration;
            _textFlowContainer.AddText(text.ToString());
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            
            Scale = new Vector2(1.0f, 0.0f);

            AddInternal(_background = new Box
            {
                Colour = new Color4(0f,0f,0f,.5f),
                RelativeSizeAxes = Axes.Both
            });
            AddInternal(_textFlowContainer);
        }

        public override void Show()
        {
            this.FadeInFromZero(100);
            this.ScaleTo(new Vector2(1.0f, 1.0f), 500, Easing.OutElasticHalf);
        }

        public override void Hide()
        {
            this.FadeOut(100)
                .OnComplete(_ =>
            {
                if (Parent is NotificationOverlay container)
                    container.RemoveBigNotification(this);
            });
        }

        protected override void Update()
        {
            if (_clock.ElapsedMilliseconds > _duration)
                Hide();

            if (!_clock.IsRunning)
                _clock.Start();
        }
    }
}