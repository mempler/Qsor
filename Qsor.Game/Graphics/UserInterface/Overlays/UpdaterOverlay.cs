using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;

namespace Qsor.Game.Graphics.UserInterface.Overlays
{
    public partial class UpdaterOverlay : CompositeDrawable
    {
        private SpriteText _statusText;
        private SpriteIcon _spinningIcon;
        
        public LocalisableString Text { get => _statusText.Text; set => _statusText.Text = value; }
        
        [Resolved]
        private Updater.UpdateManager Updater { get; set; }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Anchor = Origin = Anchor.BottomCentre;
            AutoSizeAxes = Axes.Both;

            Padding = new MarginPadding(25);
            Alpha = 0;
            
            AddInternal(_spinningIcon = new SpriteIcon
            {
                Icon = FontAwesome.Solid.SyncAlt,
                Size = new Vector2(32),
                Origin = Anchor.Centre,
                Anchor = Anchor.TopCentre,
                Rotation = 0
            });
            
            _spinningIcon
                .RotateTo(360, 2500, Easing.InOutQuint)
                .Then()
                .RotateTo(0, 1) // 0 duration is not working ?
                .Loop();
            
            AddInternal(_statusText = new SpriteText
            {
                Text = "Update available, click here to update!",
                Origin = Anchor.TopCentre,
                Anchor = Anchor.Centre,
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ScaleTo(1.25f, 500, Easing.OutBack);
            
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.ScaleTo(1f, 500, Easing.OutBack);
        }

        protected override bool OnClick(ClickEvent e)
        {
            Updater.UpdateGame();
            
            return true;
        }

        public override void Show()
        {
            this.FadeIn(2500, Easing.InOutQuint);
        }
        
        public override void Hide()
        {
            this.FadeOut(2500, Easing.InOutQuint);
        }
    }
}