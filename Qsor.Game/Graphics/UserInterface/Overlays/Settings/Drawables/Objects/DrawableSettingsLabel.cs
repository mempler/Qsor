using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables.Objects
{
    public partial class DrawableSettingsLabel : DrawableSettingsObject<LocalisableString>
    {
        private SpriteText _label;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Label.BindTo(Value);
            
            AddInternal(_label = new SpriteText
            {
                Origin = Anchor.CentreLeft,

                Font = new FontUsage(size: 18),
                
                Text = Label.Value
            });

            Label.ValueChanged += e => _label.Text = e.NewValue;
        }
    
        public DrawableSettingsLabel(LocalisableString defaultValue, LocalisableString toolTip) : base(defaultValue, string.Empty, toolTip)
        {
        }
    }
}