using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Settings.Drawables
{
    public class DrawableSettingsIconSprite : CompositeDrawable
    {
        
        [Resolved]
        private DrawableSettingsToolBar ToolBar { get; set; }

        private SpriteIcon _icon = new SpriteIcon();
        
        public IconUsage Icon
        {
            get => _icon.Icon;
            set => _icon.Icon = value;
        }

        private bool _isSelected;
        public bool Selected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _icon.FadeColour(value ? Color4.White : Color4.Gray, 100);
            }
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            CornerRadius = 0;

            Width = 48;
            Height = 48;
            
            AddInternal(_icon);
            _icon.Width = 24;
            _icon.Height = 24;
            _icon.Origin = Anchor.Centre;
            _icon.Anchor = Anchor.Centre;
            _icon.Colour = Color4.Gray;
        }
        
        protected override bool OnHover(HoverEvent e)
        {
            if (Selected)
                return false;
            
            _icon.FadeColour(Color4.White, 100);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!Selected)
                _icon.FadeColour(Color4.Gray, 100);
        }

        protected override bool OnClick(ClickEvent e)
        {
            ToolBar.Select(this);
            return true;
        }
    }
}