using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK.Graphics;
using Qsor.Game.Overlays.Settings;
using Qsor.Game.Overlays.Settings.Categories;
using Qsor.Game.Overlays.Settings.Drawables;
using Qsor.Game.Overlays.Settings.Drawables.Objects;

namespace Qsor.Game.Overlays
{
    [Cached]
    public class SettingsOverlay : CompositeDrawable
    {
        private BindableList<SettingsCategoryContainer> _categories = new();

        private DrawableSettingsToolBar _toolBar;
        private DrawableSettingsMenu _menu;
        
        public readonly Bindable<SettingsCategoryContainer> SelectedCategory = new();
        
        private Box SettingsIndex;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
            
            AddInternal(SettingsIndex = new Box
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                
                Colour = Color4.Black,
                RelativeSizeAxes = Axes.X,
                Alpha = 0
            });
            
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

        private int _childHoverLength;

        internal void ObjectHovering()
        {
            if (_childHoverLength <= 0)
            {
                SettingsIndex.FadeIn(100);
            }
            
            _childHoverLength++;
        }
        
        internal void ObjectHoverLost()
        {
            _childHoverLength--;
            if (_childHoverLength > 0)
                return;
            
            _childHoverLength = 0;
            SettingsIndex.FadeOut(500);
        }

        private object MovingTo;
        public void MoveIndexTo(Drawable settingsObject)
        {
            if (MovingTo == settingsObject)
                return;

            MovingTo = settingsObject;

            var pos = settingsObject.ToSpaceOfOtherDrawable(settingsObject.DrawPosition, _menu);
            Logger.LogPrint($"${pos.Y}");
            
            SettingsIndex.MoveToY(pos.Y, 100, Easing.InOutCubic);
            SettingsIndex.ResizeHeightTo(settingsObject.DrawSize.Y, 100, Easing.InOutCubic);
        }
    }
}