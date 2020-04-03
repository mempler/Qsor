using System.Collections.Specialized;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Settings.Drawables
{
    public class DrawableSettingsMenu : CompositeDrawable
    {
        private readonly BindableList<ISettingsCategory> _categories = new BindableList<ISettingsCategory>();
        private Box Background;
        private SearchContainer _searchContainer;
        private BasicScrollContainer _scrollContainer;
        
        public DrawableSettingsMenu(BindableList<ISettingsCategory> categories)
        {
            _categories.BindTo(categories);
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Y;
            
            Margin = new MarginPadding{ Left = 48 };
            Width = 400;
            
            AddInternal(Background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Black,
                Alpha = .6f
            });
            
            AddInternal(_searchContainer = new SearchContainer
            {
                RelativeSizeAxes = Axes.Y,
                Width = 400,
            });
            
            _searchContainer.Add(_scrollContainer = new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Y,
                Width = 400,
                ScrollbarVisible = true
            });
            
            var headerContainer = new CustomizableTextContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                TextAnchor = Anchor.TopCentre,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                
                Margin = new MarginPadding{ Top = 50 }
            };

            headerContainer.AddPlaceholder(new SpriteIcon
            {
                Width = 24,
                Height = 24,
                Icon = FontAwesome.Solid.Search
            });
            
            headerContainer.AddText("Options\n", e => e.Font = new FontUsage(size: 24, weight: "Bold"));
            headerContainer.AddText("Change the way Qsor behaves\n\n\n\n", e => { e.Colour = Color4.PaleVioletRed; e.Font = new FontUsage(size: 18);});
            headerContainer.AddText($"[0] Type to search!", e => e.Font = new FontUsage(size: 24));

            _scrollContainer.ScrollContent.Add(headerContainer);
            
            _categories.CollectionChanged += (_, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var i in e.NewItems)
                    {
                        var item = (ISettingsCategory) i;

                        var settingsCategory = new DrawableSettingsCategory
                        {
                            Name = item.Name,
                            Anchor = Anchor.TopLeft,
                            Padding = new MarginPadding{ Top = 20, Bottom = 20 },
                            Width = 400
                        };
                        
                        _scrollContainer.ScrollContent.Add(settingsCategory);
                    }
                }
            };
        }
    }
}