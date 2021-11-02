using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables
{
    public class DrawableSettingsCategory : FillFlowContainer<Drawable>
    {
        private SpriteText _text;

        private readonly SettingsCategoryContainer _categoryContainer;

        public DrawableSettingsCategory(SettingsCategoryContainer category)
        {
            _categoryContainer = category;
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            _text = new SpriteText
            {
                Text = Name.ToUpper(),
                Colour = Color4.SkyBlue,
                Font = new FontUsage(size: 24, weight: "Bold"),
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight
            };

            Direction = FillDirection.Vertical;

            Add(_text);
            
            Add(_categoryContainer);
        }
    }
}