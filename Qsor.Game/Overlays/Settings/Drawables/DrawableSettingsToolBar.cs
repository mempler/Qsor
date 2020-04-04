using System.Collections.Specialized;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Settings.Drawables
{
    [Cached]
    public class DrawableSettingsToolBar : CompositeDrawable
    {
        private readonly BindableList<ISettingsCategory> _categories = new BindableList<ISettingsCategory>();
        private BasicScrollContainer _scrollContainer;
        private SearchContainer<DrawableSettingsIconSprite> _iconFlowContainer;

        private Bindable<DrawableSettingsIconSprite> SelectedSprite = new Bindable<DrawableSettingsIconSprite>();

        public DrawableSettingsToolBar(BindableList<ISettingsCategory> categories)
        {
            _categories.BindTo(categories);
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Y;
            Width = 48;
            
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Black
            });
            
            AddInternal(_scrollContainer = new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
            });

            _scrollContainer.ScrollbarVisible = false;
            _scrollContainer.ScrollContent.AutoSizeAxes = Axes.Y;
            _scrollContainer.ScrollContent.RelativeSizeAxes = Axes.X;
            _scrollContainer.ScrollContent.Origin = Anchor.Centre;
            _scrollContainer.ScrollContent.Anchor = Anchor.Centre;
            
            _scrollContainer.ScrollContent.Add(_iconFlowContainer = new SearchContainer<DrawableSettingsIconSprite>
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical
            });

            _categories.CollectionChanged += (_, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var i in e.NewItems)
                    {
                        var item = (ISettingsCategory) i;
                        
                        var settingsIconSprite = new DrawableSettingsIconSprite
                        {
                            Name = item.Name,
                            Icon = item.Icon,
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Colour = Color4.LightGray,
                        };

                        _iconFlowContainer.Add(settingsIconSprite);
                    }
                }
            };

            SelectedSprite.ValueChanged += e =>
            {
                if (e.OldValue != null)
                    e.OldValue.Selected = false;
                
                e.NewValue.Selected = true;
            };
        }

        public void Default()
        {
            var sprite = _iconFlowContainer.FirstOrDefault(s => s.Name == "General");
            if (sprite == null)
                return;

            //_selector.FadeInFromZero(250);
            Select(sprite);
            //_selector.Position = sprite.Position;
        }
        
        public void Select(DrawableSettingsIconSprite sprite) => SelectedSprite.Value = sprite;
    }
}