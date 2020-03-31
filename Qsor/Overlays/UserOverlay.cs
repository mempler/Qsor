using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;
using Qsor.Online;
using Qsor.Online.Users;
using Qsor.Online.Users.Drawables;
using Qsor.Overlays.Drawables;

namespace Qsor.Overlays
{
    public class UserOverlay : CompositeDrawable
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
            
            _textFlowContainer = new TextFlowContainer { AutoSizeAxes = Axes.Both };
            _textFlowContainer.Padding = new MarginPadding(4);
            _textFlowContainer.AddText($"{_activeUser.Username}\n", text =>
            {
                text.Font = new FontUsage(size: 24, weight: "bold");
            });
            _textFlowContainer.AddText($"Performance: {_activeUser.Statistics.PerformancePoints}pp\n", text =>
            {
                text.Font = new FontUsage(size: 14);
            });
            _textFlowContainer.AddText($"Accuracy: {_activeUser.Statistics.Accuracy}%\n", text =>
            {
                text.Font = new FontUsage(size: 14);
            });
            _textFlowContainer.AddText($"Level: {_activeUser.Statistics.GetLevel()}", text =>
            {
                text.Font = new FontUsage(size: 14);
            });
            
            _textFlowContainer.OriginPosition = new Vector2(-75,0);

            AddInternal(_background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.White,
                Alpha = .1f
            });
            AddInternal(_avatar = new DrawableAvatar());
            AddInternal(new SpriteText
            {
                Font = new FontUsage(size: 32, weight: "Bold"),
                Colour = Color4.DarkGray,
                Text = new LocalisedString("#1"),
                
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Padding = new MarginPadding(0),
                Position = new Vector2(0, -5)
            });
            AddInternal(_textFlowContainer);
            AddInternal(_levelBar = new DrawableLevelBar(_activeUser)
            {
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
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
            _background.FadeTo(.1f, 50);
        }
    }
}