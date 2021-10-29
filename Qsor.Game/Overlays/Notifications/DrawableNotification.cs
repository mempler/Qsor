using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Notifications
{
    public class DrawableNotification : CompositeDrawable
    {
        private readonly StopwatchClock _clock;
        private readonly Action _clickAction;
        private readonly ColourInfo _borderColour;
        private readonly TextFlowContainer _textFlowContainer;
        private readonly double _duration;

        /// <summary>
        /// Drawable Notification
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colourInfo"></param>
        /// <param name="duration">Hide after X amount of MS, -1 = PositiveInfinity</param>
        /// <param name="clickAction"></param>
        public DrawableNotification(LocalisableString text, ColourInfo colourInfo, int duration = -1, Action clickAction = null)
        {
            Margin = new MarginPadding(6);
            
            _clock = new StopwatchClock();
            _clickAction = clickAction;
            _borderColour = colourInfo;
            _textFlowContainer = new TextFlowContainer
            {
                Direction = FillDirection.Full,
                AutoSizeAxes = Axes.Both,
                MaximumSize = new Vector2(290, float.MaxValue),
                Padding = new MarginPadding(10)
            };
            _duration = duration == -1 ? Math.Max(3000, (double) (text.ToString().Length * 100)) : duration;

            _textFlowContainer.AddText(text.ToString());
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Masking = true;
            BorderThickness = 2;
            BorderColour = _borderColour;
            CornerRadius = 8;
            AutoSizeAxes = Axes.Y;
            Width = 300;

            AddInternal(new Box
            {
                Colour = new Color4(0f,0f,0f,.8f),
                RelativeSizeAxes = Axes.Both
            });
            AddInternal(_textFlowContainer);
        }

        public void FadeBorder(ColourInfo newColour, double duration = 0, Easing easing = Easing.None)
            => this.TransformTo(nameof(BorderColour), newColour.AverageColour, duration, easing);
        
        protected override bool OnHover(HoverEvent e)
        {
            FadeBorder(Color4.White, 100);
            
            _clock.Stop();
            
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            FadeBorder(_borderColour, 100);

            if (_clock.ElapsedMilliseconds > _duration)
                OnClick(null);
        }

        private bool _gotClicked;
        
        protected override bool OnClick(ClickEvent e)
        {
            if (_gotClicked)
                return false;

            if (e != null)
                _clickAction?.Invoke();
            
            _gotClicked = true;
            
            this.FadeOutFromOne(250).Finally(_ =>
            {
                if (Parent is FillFlowContainer<DrawableNotification> container)
                    container.Remove(this);
            });
            
            return true;
        }

        protected override void Update()
        {
            if (_clock.ElapsedMilliseconds > _duration)
                OnClick(null);

            if (!_clock.IsRunning && !IsHovered)
                _clock.Start();
            
            base.Update();
        }
    }
}