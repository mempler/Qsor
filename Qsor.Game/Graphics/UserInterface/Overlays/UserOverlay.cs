using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;
using Qsor.Game.Graphics.UserInterface.Online.Drawables;
using Qsor.Game.Online;
using Qsor.Game.Online.Users;

namespace Qsor.Game.Graphics.UserInterface.Overlays
{
    public partial class UserOverlay : CompositeDrawable
    {
        private TextFlowContainer _textFlowContainer;
        private DrawableAvatar _avatar;
        private DrawableLevelBar _levelBar;
        private User _activeUser;
        private Box _background;
        
        [BackgroundDependencyLoader]
        private void Load(UserManager userManager)
        {
            _activeUser = userManager.User;
            
            Width = 256;
            Height = 80;
            
            _textFlowContainer = new TextFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Padding = new MarginPadding(4),
                OriginPosition = new Vector2(-75,0)
            };

            _textFlowContainer.AddParagraph($"{_activeUser.Username}", text => text.Font = new FontUsage(size: 24, weight: "Bold"));
            _textFlowContainer.AddParagraph($"Performance: {_activeUser.Statistics.PerformancePoints}pp", text => text.Font = new FontUsage(size: 14));
            _textFlowContainer.AddParagraph($"Accuracy: {_activeUser.Statistics.Accuracy:0.00}%", text => text.Font = new FontUsage(size: 14));
            _textFlowContainer.AddParagraph($"Level: {_activeUser.Statistics.GetLevel()}", text => text.Font = new FontUsage(size: 14));

            AddRangeInternal(new Drawable[]
            {
                _background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White,
                    Alpha = 0f
                },
                _avatar = new DrawableAvatar(),
                new SpriteText
                {
                    Font = new FontUsage(size: 32, weight: "Bold"),
                    Colour = Color4.DarkGray,
                    Text = new LocalisableString("#1"),
                
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Padding = new MarginPadding(0),
                    Position = new Vector2(0, -5)
                },
                _textFlowContainer,
                _levelBar = new DrawableLevelBar(_activeUser)
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                }
            });

            _avatar.User.Value = _activeUser;
        }

        protected override bool OnHover(HoverEvent e)
        {
            _background.FadeTo(.3f, 50);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            _background.FadeTo(0f, 50);
        }
    }
}