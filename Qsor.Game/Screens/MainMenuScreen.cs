using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using Qsor.Game.Beatmaps;
using Qsor.Game.Database;
using Qsor.Game.Gameplay.osu.Screens;
using Qsor.Game.Graphics.Containers;
using Qsor.Game.Overlays;
using Qsor.Game.Screens.Menu;

namespace Qsor.Game.Screens
{
    public class MainMenuScreen : Screen
    {
        private BackgroundImageContainer _background;
        private QsorLogo _qsorLogo;
        private Toolbar _toolbar;
        private BottomBar _bottomBar;
        
        private Bindable<WorkingBeatmap> _workingBeatmap = new();
        
        [Resolved]
        private NotificationOverlay NotificationOverlay { get; set; }
        
        [Resolved]
        private Storage Storage { get; set; }
        
        [Resolved]
        private Updater.Updater Updater { get; set; }

        [Resolved] 
        private GameHost Host { get; set; }
        
        [BackgroundDependencyLoader]
        private void Load(UpdaterOverlay updaterOverlay, AudioManager audioManager, QsorDbContextFactory ctxFactory, BeatmapManager beatmapManager)
        {
            var parallaxBack = new ParallaxContainer
            {
                ParallaxAmount = 0.005f
            };
            parallaxBack._content.Add(_background = new BackgroundImageContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
            AddInternal(parallaxBack);
            
            
            
            
            var db = ctxFactory.Get();
            var beatmapModel = db.Beatmaps.ToList().OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            var beatmapStorage = Storage.GetStorageForDirectory(beatmapModel?.Path);
            beatmapManager.LoadBeatmap(beatmapStorage, beatmapModel?.File);
            LoadComponent(beatmapManager.WorkingBeatmap.Value);
            _workingBeatmap.BindTo(beatmapManager.WorkingBeatmap);
            
            _background.SetTexture(_workingBeatmap.Value.Background);
            
            audioManager.AddItem(_workingBeatmap.Value.Track);
            
            
            
            
            var parallaxFront = new ParallaxContainer
            {
                ParallaxAmount = -0.03f,
                RelativeSizeAxes = Axes.Both,
            };
            parallaxFront._content.Add(new DrawSizePreservingFillContainer
            {
                
                Child = _qsorLogo = new QsorLogo
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Scale = new Vector2(1f) * (DrawSize.X / DrawSize.Y)
                }
            });
            
            AddInternal(parallaxFront);

            
            
            AddInternal(_toolbar = new Toolbar());
            
            AddInternal(_bottomBar = new BottomBar());
            
            AddInternal(updaterOverlay);
        }
        
        protected override void LoadComplete()
        {
            _workingBeatmap.Value.Play();
            
            base.LoadComplete();
        }
        
        public override void OnEntering(IScreen last)
        {
            _clock.Start();
            
            this.FadeInFromZero(2500, Easing.InExpo).Finally(e =>
            {
                NotificationOverlay.AddNotification(new LocalisableString(
                        "Please note that the client is still in a very early alpha, bugs will most likely occur! " +
                        "Consider reporting each of them in #bug-reports in it hasn't been found already."),
                    Color4.Orange, 10000);

                NotificationOverlay.AddNotification(new LocalisableString(
                        "You can play different beatmaps by editing \"game.ini\" config file. " +
                        "To open the Qsor configuration directory, click this notification!"),
                    Color4.Orange, 10000, Storage.OpenInNativeExplorer);

                NotificationOverlay.AddNotification(new LocalisableString(
                        $"You're currently running {QsorBaseGame.Version}! " +
                        "Click here to view the changelog."),
                    Color4.Gray, 10000, () => Host.OpenUrlExternally($"https://github.com/osuAkatsuki/Qsor/releases/tag/{QsorBaseGame.Version}"));

                if (!DebugUtils.IsDebugBuild)
                    Updater.CheckAvailable();
            });
        }

        public override bool OnExiting(IScreen next)
        {
            this.FadeOutFromOne(2500, Easing.OutExpo);
            return true;
        }

        private StopwatchClock _clock = new();
        public bool IsFading;
        
        protected override void Update()
        {
            if (IsFading || _clock.ElapsedMilliseconds <= 5000)
                return;
            
            _toolbar.FadeOut(13000);
            _bottomBar.FadeOut(13000);
            
            IsFading = true;
        }
        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            _toolbar.ClearTransforms();
            _bottomBar.ClearTransforms();
            
            _toolbar.FadeIn(250);
            _bottomBar.FadeIn(250);
            
            IsFading = false;
            _clock.Restart();
            return true;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (_qsorLogo.IsHovered)
            {
                var game = (QsorBaseGame) Game;
                game.PushScreen(new OsuScreen
                {
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    FillMode = FillMode.Fill,
                });
            }

            return base.OnClick(e);
        }
    }
}