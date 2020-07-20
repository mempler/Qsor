using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Drawables
{
    public class DrawableTooltip : CompositeDrawable
    {
        public readonly Bindable<LocalisedString> Value = new Bindable<LocalisedString>(string.Empty);

        private TextFlowContainer _textFlowContainer;
        
        public DrawableTooltip(Bindable<LocalisedString> value)
        {
            Value.BindTo(value);
        }

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

            AddInternal(new Box
            {
                Colour = Color4.Black,
                Alpha = .5f,
                RelativeSizeAxes = Axes.Both,
            });
            AddInternal(_textFlowContainer = new TextFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                MaximumSize = new Vector2(400, float.MaxValue),
                Text = Value.Value
            });

            Value.ValueChanged += e => _textFlowContainer.Text = e.NewValue;
        }
    }
}