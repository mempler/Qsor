using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osuTK;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables
{
    public class DrawableSettingsSubCategory : CompositeDrawable
    {
        public Bindable<LocalisableString> Label = new(string.Empty);
        private SpriteText _label;
        
        public FillFlowContainer Content { get; } = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Spacing = new Vector2(0, 2.5f),
            Margin = new MarginPadding { Left = 10, Top = 50 }
        };
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Y,
                Width = 2,
                Origin = Anchor.Centre,
                Anchor = Anchor.CentreLeft,
                Alpha = .2f
            });
            AddInternal(_label = new SpriteText
            {
                Text = Label.Value,
                Font = new FontUsage(size: 22, weight: "bold"),
                Margin = new MarginPadding{ Left = 10, Top = 5 }
            });

            Label.ValueChanged += e => _label.Text = e.NewValue;
            AddInternal(Content);
        }

        public DrawableSettingsSubCategory(LocalisableString label)
        {
            Label.Default = label;
            Label.SetDefault();
        }
    }
}