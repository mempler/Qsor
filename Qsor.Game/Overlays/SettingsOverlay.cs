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
        }

        protected override void LoadComplete()
        {
            _toolBar.Default();
        }

        public void AddCategory(SettingsCategoryContainer category)
        {
            _categories.Add(category);
        }

        public override void Show()
        {
            this.FadeInFromZero(1000, Easing.In);
        }

        public override void Hide()
        {
            this.FadeOutFromOne(1000, Easing.Out);
        }
    }
}