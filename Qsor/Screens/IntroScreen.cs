using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Screens
{
    public class IntroScreen : Screen
    {
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
            
            var warningIcon = _textFlowContainer.AddPlaceholder(new SpriteIcon
            {
                Icon = FontAwesome.Solid.ExclamationTriangle,
                
                Size = new Vector2(48),
                
                Colour = Color4.Yellow,
            });

            _textFlowContainer.AddText($"[{warningIcon}]\n");
            _textFlowContainer.AddText("Warning!\n", text => { text.Colour = Color4.Yellow; text.Font = new FontUsage(size: 32, weight: "Bold");});
            _textFlowContainer.AddText("This game is currently in really early alpha! Bugs to be expected.\n");
            _textFlowContainer.AddText("Please consider reporting them all, no matter how small they are.\n");
            
            AddInternal(_textFlowContainer);
        }

        public override void OnEntering(IScreen last)
        {
            for (var i = 0; i < _textFlowContainer.Count; i++)
                _textFlowContainer[i].FadeInFromZero(250 + 100 * i);
            
            Scheduler.AddDelayed(() => _textFlowContainer[0].FlashColour(Color4.White, 1000), 1000, true);
            
            base.OnEntering(last);
        }

        public override bool OnExiting(IScreen next)
        {
            _textFlowContainer.FadeOutFromOne(500);

            return true;
        }
    }
}
