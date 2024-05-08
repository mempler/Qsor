using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Graphics.Containers
{
    public partial class QsorTooltipContainer : TooltipContainer
    {
        protected override ITooltip CreateTooltip() => new QsorTooltip();

        protected override double AppearDelay => 120;

        public QsorTooltipContainer(CursorContainer cursor)
            : base(cursor)
        {
        }
        
        public partial class QsorTooltip : Tooltip
        {
            private TextFlowContainer _textFlowContainer;

            [BackgroundDependencyLoader]
            private void Load()
            {
                CornerRadius = 4f;

                Masking = true;
                BorderColour = Color4.Gray;
                BorderThickness = 2;

                Scale = new Vector2(.8f);

                AutoSizeAxes = Axes.Both;
                Alpha = 0;

                Children = new Drawable[]
                {
                    new Box
                    {
                        Colour = Color4.Black,
                        RelativeSizeAxes = Axes.Both,
                    },
                    _textFlowContainer = new TextFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        MaximumSize = new Vector2(400, float.MaxValue),
                    }
                };
            }

            private string _cachedString;
            public override void SetContent(LocalisableString content)
            {
                var contentS = content.ToString();
                if (_cachedString == contentS)
                    return;
                
                _textFlowContainer.Text = contentS;
                _cachedString = contentS;
            }

            protected override void PopIn()
            {
                this.FadeIn(100, Easing.In);
            }
            
            protected override void PopOut()
            {
                this.FadeOut(100, Easing.In);
            }
        }
    }
}