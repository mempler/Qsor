using System;
using System.Collections.Specialized;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using Qsor.Game.Overlays.Settings;
using Qsor.Game.Overlays.Settings.Categories;
using Qsor.Game.Overlays.Settings.Drawables;

namespace Qsor.Game.Overlays
{
    public class SettingsOverlay : CompositeDrawable
    {
        private BindableList<ISettingsCategory> _categories = new BindableList<ISettingsCategory>();

        private DrawableSettingsToolBar _toolBar;

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Y;
            
            AddInternal(_toolBar = new DrawableSettingsToolBar(_categories));
            
            AddCategory(new SettingsGeneralCategory());
            AddCategory(new SettingsGraphicsCategory());
            AddCategory(new SettingsGameplayCategory());
            AddCategory(new SettingsAudioCategory());
            AddCategory(new SettingsSkinCategory());
            AddCategory(new SettingsInputCategory());
            AddCategory(new SettingsEditorCategory());
            AddCategory(new SettingsOnlineCategory());
            AddCategory(new SettingsMaintenanceCategory());
            
            Scheduler.AddDelayed(_toolBar.Default, 5);
        }

        public void AddCategory(ISettingsCategory category)
        {
            _categories.Add(category);
        }

        public override void Show()
        {
            
        }

        public override void Hide()
        {
            
        }
    }
}