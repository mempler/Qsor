using System.Collections.Specialized;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables
{
    [Cached]
    public class DrawableSettingsMenu : CompositeDrawable
    {
        private readonly BindableList<SettingsCategoryContainer> _categories = new();
        private SearchContainer _searchContainer;
        private BasicScrollContainer _scrollContainer;

        public DrawableSettingsMenu(BindableList<SettingsCategoryContainer> categories)
        {
            _categories.BindTo(categories);
        }

        [BackgroundDependencyLoader]
        private void Load(SettingsOverlay settingsOverlay)
        {
            Masking = true;
            RelativeSizeAxes = Axes.Y;
            
            Margin = new MarginPadding{ Left = 48 };
            Width = 400;
            
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Black,
                Alpha = .6f
            });
 
            AddInternal(_scrollContainer = new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                ScrollbarVisible = true
            });
            
            var headerContainer = new CustomizableTextContainer
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                TextAnchor = Anchor.TopCentre,
                //RelativeSizeAxes = Axes.X,
                Width = 400,
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
            headerContainer.AddText("[0] Type to search!", e => e.Font = new FontUsage(size: 24));

            _scrollContainer.ScrollContent.Add(headerContainer);
            
            _scrollContainer.ScrollbarVisible = false;
            _scrollContainer.ScrollContent.AutoSizeAxes = Axes.Y;
            _scrollContainer.ScrollContent.RelativeSizeAxes = Axes.X;
            
            _scrollContainer.ScrollContent.Add(_searchContainer = new SearchContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Margin = new MarginPadding{ Top = 150 },
            });

            settingsOverlay.SelectedCategory.ValueChanged += e =>
            {
                // Little hack to make it feel smoother
                if (_categories.FirstOrDefault() == e.NewValue)
                {
                    _scrollContainer.ScrollToStart();
                }
                else if (_categories.LastOrDefault() == e.NewValue)
                {
                    _scrollContainer.ScrollToEnd();
                }
                else
                {
                    _scrollContainer.ScrollTo(e.NewValue);
                }
            };

            _categories.CollectionChanged += (_, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var i in e.NewItems)
                    {
                        var item = (SettingsCategoryContainer) i;

                        var settingsCategory = new DrawableSettingsCategory(item)
                        {
                            Name = item.Name,
                            Anchor = Anchor.TopLeft,
                            Padding = new MarginPadding(20),
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y
                        };
                        
                        _searchContainer.Add(settingsCategory);
                    }
                }
            };
        }
    }
}