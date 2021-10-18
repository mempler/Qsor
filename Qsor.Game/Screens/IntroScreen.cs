using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Screens
{
    public class IntroScreen : Screen
    {
        private SpriteIcon _spriteIcon;
        private CustomizableTextContainer _textFlowContainer;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            _textFlowContainer = new CustomizableTextContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                
                RelativeSizeAxes = Axes.Both,
                TextAnchor = Anchor.Centre,
            };
            
            var warningIcon = _textFlowContainer.AddPlaceholder(_spriteIcon = new SpriteIcon
            {
                Icon = FontAwesome.Solid.ExclamationTriangle,
                Size = new Vector2(48),
                Colour = Color4.Yellow,
            });

            _textFlowContainer.AddParagraph($"[{warningIcon}]");
            _textFlowContainer.AddParagraph("Warning!", text =>
            {
                text.Colour = Color4.Yellow;
                text.Font = new FontUsage(size: 32, weight: "Bold");
            });
            _textFlowContainer.AddParagraph("This game is currently in really early alpha! Bugs to be expected.");
            _textFlowContainer.AddParagraph("Please consider reporting them all, no matter how small they are.");
            
            AddInternal(_textFlowContainer);
        }

        public override void OnEntering(IScreen last)
        {
            for (var i = 0; i < _textFlowContainer.Count; i++)
                _textFlowContainer[i].FadeInFromZero(250 + 100 * i);

            Scheduler.AddDelayed(() => _spriteIcon.FlashColour(Color4.White, 1000), 1000, true);
        }

        public override bool OnExiting(IScreen next)
        {
            _textFlowContainer.FadeOutFromOne(500);

            return true;
        }
    }
}
