using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Settings.Drawables.Objects
{
    public class DrawableSettingsCheckbox : DrawableSettingsObject<bool>
    {
        private SpriteText _label;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            
            AddInternal(new DrawableSettingsCheckboxNode(Value));
            AddInternal(_label = new SpriteText
            {
                Origin = Anchor.CentreLeft,
                
                Margin = new MarginPadding{ Left = 18f },
                
                Font = new FontUsage(size: 18),
                Text = Label.Value
            });

            Label.ValueChanged += e => _label.Text = e.NewValue;
        }

        public DrawableSettingsCheckbox(bool defaultValue, LocalisedString label, LocalisedString toolTip) : base(defaultValue, label, toolTip)
        {
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (Value.Disabled)
                return false;
            
            Value.Value = !Value.Value;
            return true;
        }
        
        private class DrawableSettingsCheckboxNode : CompositeDrawable
        {
            public readonly Bindable<bool> Checked = new Bindable<bool>();

            public DrawableSettingsCheckboxNode(Bindable<bool> checkedBindable)
            {
                Checked.BindTo(checkedBindable);
            }
            
            private Box _box;
            
            [BackgroundDependencyLoader]
            private void Load()
            {
                Width = 16;
                Height = 16;

                Origin = Anchor.Centre;

                Masking = true;
                BorderColour = Color4.PaleVioletRed;
                BorderThickness = 2.5f;
                
                CornerRadius = Width / 2f;
                
                AddInternal(_box = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.PaleVioletRed,
                    
                    Alpha = Checked.Value ? 1 : 0,
                    AlwaysPresent = true
                });

                Checked.ValueChanged += e => _box.FadeTo(e.NewValue ? 1 : 0, 100, Easing.In);
                Checked.DisabledChanged += e =>
                {
                    _box.Colour = e ? Color4.DimGray : Color4.PaleVioletRed;
                    BorderColour = e ? Color4.Gray : Color4.PaleVioletRed;
                };
            }
        }
    }
}