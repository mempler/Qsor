using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK.Graphics;
using Qsor.Game.Overlays.Drawables;

namespace Qsor.Game.Overlays.Settings.Drawables.Objects
{
    public abstract class DrawableSettingsObject<T> : CompositeDrawable, IRequireHighFrequencyMousePosition
    {
        public readonly Bindable<LocalisableString> Label = new(string.Empty);
        public readonly Bindable<LocalisableString> ToolTip = new(string.Empty);
        public readonly Bindable<T> Value;

        private Box _hoverBox;
        private DrawableTooltip _tooltip;

        public DrawableSettingsObject(T defaultValue, LocalisableString label, LocalisableString toolTip)
        {
            Label.Value = label;
            ToolTip.Value = toolTip;
            
            Value = new Bindable<T>(defaultValue);

            Value.SetDefault();
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;

            Height = 32;
            Padding = new MarginPadding(10);
            
            AddInternal(_hoverBox = new Box
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                
                Height = 32,
                Width = 450,

                Colour = Color4.Black,
                
                Alpha = 0
            });
            
            AddInternal(_tooltip = new DrawableTooltip(ToolTip));
        }

        protected override bool OnHover(HoverEvent e)
        {
            _hoverBox.FadeTo(.5f, 250, Easing.In);
            _tooltip.FadeIn(100, Easing.In);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            _hoverBox.FadeOut(100, Easing.In);
            _tooltip.FadeOut(100, Easing.In);
        }
        
        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            _tooltip.Position = e.MousePosition;

            return true;
        }
    }
}