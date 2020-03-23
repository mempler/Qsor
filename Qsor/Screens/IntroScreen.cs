using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Screens
{
    public class IntroScreen : Screen
    {
        private SpriteIcon _warningIcon;
        private SpriteText _warningText;
        private SpriteText _alphaText;
        private SpriteText _bugText;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            InternalChildren = new Drawable[]
            {
                _warningIcon = new SpriteIcon
                {
                    Icon = FontAwesome.Solid.ExclamationTriangle,
                    Size = new Vector2(48),

                    Colour = Color4.Yellow,

                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    
                    Position = new Vector2(0, -90)
                },

                _warningText = new SpriteText
                {
                    Text = "Warning!",
                    
                    Colour = Color4.Yellow,
                    
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,

                    Padding = new MarginPadding { Bottom = 10 },
                    Font = new FontUsage(size: 42),

                    Position = new Vector2(0, -40),
          
                    Spacing = new Vector2(2.5f),
                },

                _alphaText = new SpriteText
                {
                    Text = "This game is currently in really early alpha! Bugs to be expected.",

                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    
                    Position = new Vector2(0, -10)
                },

                _bugText = new SpriteText
                {
                    Text = "Please consider reporting them all, no matter how small they are.",

                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,

                    Position = new Vector2(0, 10)
                }
            };
        }

        public override void OnEntering(IScreen last)
        {
            _warningIcon.FadeInFromZero(2000);
            _warningText.FadeInFromZero(4000);
            _alphaText.FadeInFromZero(5000);
            _bugText.FadeInFromZero(6000);

            Scheduler.AddDelayed(() =>
            {
                _warningIcon.FlashColour(Color4.White, 1000);
            }, 1000, true);
            
            base.OnEntering(last);
        }

        public override bool OnExiting(IScreen next)
        {
            _warningIcon.MoveToOffset(new Vector2(-60, 0), 1000);
            _warningText.MoveToOffset(new Vector2(-100, 0), 2000);
            _alphaText.MoveToOffset(new Vector2(-100, 0), 3000);
            _bugText.MoveToOffset(new Vector2(-100, 0), 4000);
            
            _warningIcon.FadeOutFromOne(1000);
            _warningText.FadeOutFromOne(1000);
            _alphaText.FadeOutFromOne(1000);
            _bugText.FadeOutFromOne(1000);

            return true;
        }
    }
}