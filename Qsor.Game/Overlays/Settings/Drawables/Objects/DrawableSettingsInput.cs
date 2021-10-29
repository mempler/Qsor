using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Settings.Drawables.Objects
{
    public class DrawableSettingsInput : DrawableSettingsObject<string>
    {
        private DrawableSettingsInputNode _node;
        private SpriteText _label;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;

            Height = 64;
            
            AddInternal(_label = new SpriteText
            {
                Origin = Anchor.CentreLeft,
                
                Font = new FontUsage(size: 18),
                Text = Label.Value
            });
            
            AddInternal(_node = new DrawableSettingsInputNode(Value)
            {
                RelativeSizeAxes = Axes.X,
                Position = new Vector2(-2, 16),
                Height = 16
            });

            Label.ValueChanged += e => _label.Text = e.NewValue;
        }

        
        public override bool AcceptsFocus => true;
        protected override void OnFocus(FocusEvent e)
        {
            Logger.LogPrint("Focus!!");
            base.OnFocus(e);
            GetContainingInputManager().ChangeFocus(null);
            GetContainingInputManager().ChangeFocus(_node);
        }

        public DrawableSettingsInput(string defaultValue, LocalisableString label, LocalisableString toolTip)
            : base(defaultValue, label, toolTip)
        {
        }

        private class DrawableSettingsInputNode : TextBox
        {
            public DrawableSettingsInputNode(Bindable<string> inputBox)
            {
            }

            [BackgroundDependencyLoader]
            private void Load()
            {
                Masking = true;
                
                CornerRadius = 3;
                CornerExponent = 2f;

                BorderColour = Color4.Gray;
                BorderThickness = 3f;

                AddInternal(new Box
                {
                    Colour = Color4.Transparent,
                    RelativeSizeAxes = Axes.Both,
                });
            }
            
            protected override void NotifyInputError()
            {
            }

            protected override SpriteText CreatePlaceholder()
            {
                return new SpriteText();
            }

            protected override Caret CreateCaret()
            {
                return new BasicTextBox.BasicCaret();
            }
        }
    }
}