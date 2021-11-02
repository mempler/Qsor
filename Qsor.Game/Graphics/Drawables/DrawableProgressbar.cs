using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace Qsor.Game.Graphics.Drawables
{
    public class DrawableProgressbar : Container, IHasCurrentValue<double>
    {
        private Box _progressBox;
        
        private readonly BindableWithCurrent<double> _current = new();

        public Bindable<double> Current
        {
            get => _current.Current;
            set => _current.Current = value;
        }

        public DrawableProgressbar()
        {
            Height = 6;
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;

            Masking = true;

            CornerRadius = 3;
            CornerExponent = 2f;
            
            AddInternal(new Box
            {
                Colour = new Color4(1f, 1f, 1f, 0.5f),
                RelativeSizeAxes = Axes.Both
            });
            
            AddInternal(_progressBox = new Box
            {
                Colour = Colour,
                RelativeSizeAxes = Axes.Y,
            });

            _current.ValueChanged += e =>
            {
                _progressBox.Width = (int) (DrawSize.X * e.NewValue);
            };
        }
    }
}