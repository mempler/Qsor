using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables.Objects
{
    public abstract partial class DrawableSettingsObject<T> : CompositeDrawable, IRequireHighFrequencyMousePosition, IHasTooltip
    {
        public readonly Bindable<LocalisableString> Label = new(string.Empty);
        public readonly Bindable<LocalisableString> ToolTip = new(string.Empty);
        public readonly Bindable<T> Value;

        public LocalisableString TooltipText => ToolTip.Value;
        
        [Resolved]
        private SettingsOverlay SettingsOverlay { get; set; }

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
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (!IsHovered)
                return base.OnMouseMove(e);
            
            SettingsOverlay.MoveIndexTo(this);
            return true;
        }

        protected override bool OnHover(HoverEvent e)
        {
            SettingsOverlay.ObjectHovering();
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            SettingsOverlay.ObjectHoverLost();
        }
    }
}