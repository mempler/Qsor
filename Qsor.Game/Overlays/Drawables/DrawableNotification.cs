using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Drawables
{
    public class DrawableNotification : CompositeDrawable
    {
        private ColourInfo _borderColour
        {
            get => BindableBorderColour.Value;
            set => BindableBorderColour.Value = value;
        }
        
        public Bindable<ColourInfo> BindableBorderColour;

        private TextFlowContainer _textFlowContainer;
        
        public DrawableNotification(LocalisedString text, ColourInfo colourInfo)
        {
            _textFlowContainer = new TextFlowContainer
            {
                Direction = FillDirection.Full,
                FillMode = FillMode.Fit,
                AutoSizeAxes = Axes.Both,
                MaximumSize = new Vector2(300, float.MaxValue),
                Padding = new MarginPadding(10)
            };

            _textFlowContainer.AddText(text);

            BindableBorderColour = new Bindable<ColourInfo> {Default = colourInfo};
            BindableBorderColour.ValueChanged += e => BorderColour = e.NewValue;
            BindableBorderColour.DefaultChanged += e => BorderColour = e.NewValue;
            BindableBorderColour.SetDefault();
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Masking = true;
            BorderThickness = 2;
            CornerRadius = 8;

            AutoSizeAxes = Axes.Both;
            
            AddInternal(new Box
            {
                Colour = new Color4(0f,0f,0f,.8f),
                RelativeSizeAxes = Axes.Both
            });
            AddInternal(_textFlowContainer);
        }

        public void FadeBorder(ColourInfo newColour, double duration = 0, Easing easing = Easing.None)
            => this.TransformTo(nameof(_borderColour), newColour, duration, easing);

        protected override bool OnHover(HoverEvent e)
        {
            FadeBorder(Color4.White, 100);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            FadeBorder(BindableBorderColour.Default, 100);
        }

        protected override bool OnClick(ClickEvent e)
        {
            this.FadeOutFromOne(250).Finally(_ =>
            {
                if (Parent is FillFlowContainer<DrawableNotification> container)
                    container.Remove(this);
            });
            return true;
        }
    }
}