using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using Qsor.Game.Overlays.Settings;
using Qsor.Game.Overlays.Settings.Categories;
using Qsor.Game.Overlays.Settings.Drawables;

namespace Qsor.Game.Overlays
{
    [Cached]
    public class SettingsOverlay : CompositeDrawable
    {
        private BindableList<SettingsCategoryContainer> _categories = new BindableList<SettingsCategoryContainer>();

        private DrawableSettingsToolBar _toolBar;
        private DrawableSettingsMenu _menu;
        
        public readonly Bindable<SettingsCategoryContainer> SelectedCategory = new Bindable<SettingsCategoryContainer>();
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
            
            AddInternal(_menu = new DrawableSettingsMenu(_categories));
            AddInternal(_toolBar = new DrawableSettingsToolBar(_categories));

            AddCategory(new SettingsGeneralCategory());
            AddCategory(new SettingsGraphicsCategory());
            
            _menu.Width = 0;
            Alpha = 0;
        }

        protected override void LoadComplete()
        {
            _toolBar.Default();
        }

        public void AddCategory(SettingsCategoryContainer category)
        {
            _categories.Add(category);
        }
        
        
        public bool IsShown { get; private set; }
        public override void Show()
        {
            IsShown = true;
            
            this.FadeIn(200);
            _menu.ResizeWidthTo(400, 400, Easing.InOutCubic);
        }

        public override void Hide()
        {
            IsShown = false;
            
            this.FadeOut(800);
            _menu.ResizeWidthTo(0, 1000, Easing.InOutCubic);
        }
    }
}