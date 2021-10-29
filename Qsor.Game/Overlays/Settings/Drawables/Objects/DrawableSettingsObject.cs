using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK.Graphics;
using Qsor.Game.Graphics.Containers;
using Qsor.Game.Overlays.Drawables;

namespace Qsor.Game.Overlays.Settings.Drawables.Objects
{
    public abstract class DrawableSettingsObject<T> : CompositeDrawable, IRequireHighFrequencyMousePosition, IHasTooltip
    {
        public readonly Bindable<LocalisableString> Label = new(string.Empty);
        public readonly Bindable<LocalisableString> ToolTip = new(string.Empty);
        public readonly Bindable<T> Value;

        public LocalisableString TooltipText => ToolTip.Value;
        
        private Box _hoverBox;

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
        }

        protected override bool OnHover(HoverEvent e)
        {
            _hoverBox.FadeTo(.5f, 250, Easing.In);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            _hoverBox.FadeOut(100, Easing.In);
        }
    }
}