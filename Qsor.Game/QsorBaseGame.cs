using System;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Qsor.Game.Beatmaps;
using Qsor.Game.Configuration;
using Qsor.Game.Database;
using Qsor.Game.Graphics.Containers;
using Qsor.Game.Graphics.UserInterface.Overlays;
using Qsor.Game.Graphics.UserInterface.Overlays.Notification;
using Qsor.Game.Graphics.UserInterface.Overlays.Settings;
using Qsor.Game.Online;
using Qsor.Game.Updater;
using Qsor.Game.Utility;

namespace Qsor.Game
{
    public partial class QsorBaseGame : osu.Framework.Game
    {
        protected readonly string[] Args;
        protected BeatmapManager BeatmapManager;
        protected UserManager UserManager;
        
        protected BeatmapMirrorAccess BeatmapMirrorAccess;
        
        protected QsorDbContextFactory QsorDbContextFactory;
        protected QsorConfigManager ConfigManager;
        
        protected NotificationOverlay NotificationOverlay;
        protected UpdaterOverlay UpdaterOverlay;
        protected SentryLogger SentryLogger;
        protected SettingsOverlay SettingsOverlay;
        protected TooltipContainer TooltipContainer;
        
        public Updater.UpdateManager Updater;
        
        private DependencyContainer _dependencies;
        
        private ScreenStack _stack;

        public static string Version => !DebugUtils.IsDebugBuild
            ? Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3)
            : DateTime.Now.ToString("yyyy.Mdd.0") + (DebugUtils.IsDebugBuild ? " Debug Build" : string.Empty);

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            _dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public QsorBaseGame(string[] args)
        {
            Args = args;
        }
        
        [BackgroundDependencyLoader]
        private void Load(Storage storage)
        {
            // Load important dependencies
            {
                _dependencies.Cache(UserManager = new UserManager());
            
                _dependencies.Cache(BeatmapMirrorAccess = new BeatmapMirrorAccess());
                Dependencies.Inject(BeatmapMirrorAccess);

                _dependencies.Cache(QsorDbContextFactory = new QsorDbContextFactory(storage));
                _dependencies.Cache(ConfigManager = new QsorConfigManager(storage));
            
                _dependencies.Cache(NotificationOverlay = new NotificationOverlay());
            
                _dependencies.Cache(SentryLogger = new SentryLogger(this));
            
                _dependencies.CacheAs(this);
                _dependencies.CacheAs(Host);
                
                _dependencies.Cache(BeatmapManager = new BeatmapManager());
                Dependencies.Inject(BeatmapManager);
                
                Updater ??= CreateUpdater();
                UpdaterOverlay = new UpdaterOverlay();
            
                _dependencies.Cache(UpdaterOverlay);
                _dependencies.CacheAs(Updater);
                
                LoadComponent(Updater);
            }

            // Add our Resources/ directory
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
            
            // Migrate the given database
            QsorDbContextFactory.Get().Migrate();
            
            // Root container for everything and anything, we have QsorToolTipContainer as root
            // so we can display our tooltips cleanly.
            Add(TooltipContainer = new QsorTooltipContainer(null)
            {
                RelativeSizeAxes = Axes.Both,
            });
            
            // Update config (if necessary)
            ConfigManager.Save();
            
            _stack = new ScreenStack();
            
            TooltipContainer.Add(_stack);
            TooltipContainer.Add(SettingsOverlay = new SettingsOverlay());
            TooltipContainer.Add(NotificationOverlay);
        }

        protected virtual Updater.UpdateManager CreateUpdater() => new DummyUpdater();

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing) {
                ConfigManager?.Dispose();
                NotificationOverlay?.Dispose();
                
                SentryLogger?.Dispose();
            }

            base.Dispose(isDisposing);
        }
        
        public void PushScreen(Screen screen)
        {
            _stack.Push(screen);
        }

        public void ExitScreen()
        {
            _stack.Exit();
        }
    }
}