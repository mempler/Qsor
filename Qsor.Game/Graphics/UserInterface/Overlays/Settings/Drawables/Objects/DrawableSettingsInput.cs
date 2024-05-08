using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables.Objects
{
    public partial class DrawableSettingsInput : DrawableSettingsObject<string>
    {
        private bool _isPassword;
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
            
            AddInternal(_node = new DrawableSettingsInputNode(Value, _isPassword)
            {
                RelativeSizeAxes = Axes.X,
                Position = new Vector2(-2, 16),
                Height = 22
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

        public DrawableSettingsInput(string defaultValue, LocalisableString label, LocalisableString toolTip,
            bool isPassword = false)
            : base(defaultValue, label, toolTip)
        {
            _isPassword = isPassword;
        }

        private partial class DrawableSettingsInputNode : TextBox
        {
            private readonly bool _isPassword;
            
            protected override bool AllowClipboardExport => !_isPassword;
            protected override bool AllowWordNavigation => !_isPassword;
            protected override Drawable AddCharacterToFlow(char c) =>
                base.AddCharacterToFlow(_isPassword ? '*' : c);            
            
            public DrawableSettingsInputNode(Bindable<string> inputBox, bool isPassword)
            {
                _isPassword = isPassword;
            }

            [BackgroundDependencyLoader]
            private void Load()
            {
                Masking = true;
                
                //CornerRadius = 3;
                //CornerExponent = 2f;

                BorderColour = Color4.DarkGray;
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

            protected override void OnFocus(FocusEvent e)
            {
                this.TransformTo(nameof(BorderColour), (SRGBColour)Color4.White, 100);
                base.OnFocus(e);
            }

            protected override void OnFocusLost(FocusLostEvent e)
            {
                this.TransformTo(nameof(BorderColour), (SRGBColour)Color4.DarkGray, 100);
                base.OnFocusLost(e);
            }
        }
    }
}