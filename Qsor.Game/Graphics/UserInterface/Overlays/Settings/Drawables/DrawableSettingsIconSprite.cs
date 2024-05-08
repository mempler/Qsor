using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables
{
    public partial class DrawableSettingsIconSprite : CompositeDrawable
    {
        
        [Resolved]
        private DrawableSettingsToolBar ToolBar { get; set; }

        private readonly Bindable<SettingsCategoryContainer> _selectedCategory = new();

        private readonly SpriteIcon _icon = new();
        private Box _selector;

        
        public IconUsage Icon
        {
            get => _icon.Icon;
            set => _icon.Icon = value;
        }

        public SettingsCategoryContainer Category { get; }

        public DrawableSettingsIconSprite(SettingsCategoryContainer category)
        {
            Category = category;
        }

        [BackgroundDependencyLoader]
        private void Load(SettingsOverlay settingsOverlay)
        {
            _selectedCategory.BindTo(settingsOverlay.SelectedCategory);
            CornerRadius = 0;

            Width = 48;
            Height = 48;
            
            AddInternal(_icon);
            _icon.Width = 24;
            _icon.Height = 24;
            _icon.Origin = Anchor.Centre;
            _icon.Anchor = Anchor.Centre;
            _icon.Colour = Color4.Gray;
            
            AddInternal(_selector = new Box
            {
                Colour = Color4.HotPink,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.Centre,
                Width = 4,
                Height = 48,
                Margin = new MarginPadding { Right = 2.5f },
                Alpha = 0,
            });

            _selectedCategory.ValueChanged += e =>
            {
                _icon.FadeColour(e.NewValue == Category ? Color4.White : Color4.Gray, 100);
                _selector.FadeTo(e.NewValue == Category ? 1f : 0f, 250);
            };
        }
        
        protected override bool OnHover(HoverEvent e)
        {
            if (_selectedCategory.Value == Category)
                return false;
            
            _icon.FadeColour(Color4.White, 100);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (_selectedCategory.Value != Category)
                _icon.FadeColour(Color4.Gray, 100);
        }

        protected override bool OnClick(ClickEvent e)
        {
            _selectedCategory.Value = Category;
            return true;
        }
    }
}