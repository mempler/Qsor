using System;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Qsor.Game.Beatmaps;
using Qsor.Game.Configuration;
using Qsor.Game.Database;
using Qsor.Game.Discord;
using Qsor.Game.Online;
using Qsor.Game.Overlays;
using Qsor.Game.Updater;
using Qsor.Game.Utils;

namespace Qsor.Game
{
    public class QsorBaseGame : osu.Framework.Game
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
        protected DiscordManager DiscordManager;
        protected SettingsOverlay SettingsOverlay;
        
        public Updater.Updater Updater;
        
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
            _dependencies.Cache(BeatmapManager = new BeatmapManager());
            _dependencies.Cache(UserManager = new UserManager());
            
            _dependencies.Cache(BeatmapMirrorAccess = new BeatmapMirrorAccess());
            Dependencies.Inject(BeatmapMirrorAccess);

            _dependencies.Cache(QsorDbContextFactory = new QsorDbContextFactory(storage));
            _dependencies.Cache(ConfigManager = new QsorConfigManager(storage));
            
            _dependencies.Cache(NotificationOverlay = new NotificationOverlay());
            
            _dependencies.Cache(SentryLogger = new SentryLogger(this));
            
            _dependencies.Cache(DiscordManager = new DiscordManager());
            AddInternal(DiscordManager);

            _dependencies.CacheAs(this);
            _dependencies.CacheAs(Host);
            
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
            
            QsorDbContextFactory.Get().Migrate();
            
            Dependencies.Inject(BeatmapManager);
            
            AddInternal(NotificationOverlay);
            
            Updater ??= new DummyUpdater();
            UpdaterOverlay = new UpdaterOverlay();
            
            _dependencies.Cache(UpdaterOverlay);
            _dependencies.CacheAs(Updater);
            
            LoadComponent(Updater);
            
            ConfigManager.Save();
            
            AddInternal(SettingsOverlay = new SettingsOverlay());
            
            _stack = new ScreenStack(true);
            Add(_stack);
        }

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