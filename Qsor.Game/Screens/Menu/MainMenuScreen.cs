using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
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

namespace Qsor.Game.Screens.Menu
{
    public class MainMenuScreen : Screen
    {
        private BackgroundImageContainer _background;
        private QsorLogo _qsorLogo;
        private Toolbar Toolbar;
        private BottomBar bottomBar;
        
        private Bindable<WorkingBeatmap> WorkingBeatmap = new Bindable<WorkingBeatmap>();
        
        [Resolved]
        private NotificationOverlay NotificationOverlay { get; set; }
        
        [BackgroundDependencyLoader]
        private void Load(AudioManager audioManager, Storage storage, QsorDbContextFactory ctxFactory, BeatmapManager beatmapManager)
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
            var beatmapStorage = storage.GetStorageForDirectory(beatmapModel?.Path);
            beatmapManager.LoadBeatmap(beatmapStorage, beatmapModel?.File);
            LoadComponent(beatmapManager.WorkingBeatmap.Value);
            WorkingBeatmap.BindTo(beatmapManager.WorkingBeatmap);
            
            _background.SetTexture(WorkingBeatmap.Value.Background);
            
            audioManager.AddItem(WorkingBeatmap.Value.Track);
            
            
            
            
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

            
            
            AddInternal(Toolbar = new Toolbar());
            
            AddInternal(bottomBar = new BottomBar());
        }
        
        protected override void LoadComplete()
        {
            WorkingBeatmap.Value.Play();
            
            base.LoadComplete();
        }
        
        public override void OnEntering(IScreen last)
        {
            clock.Start();
            this.FadeInFromZero(2500, Easing.InExpo).Finally(e =>
            {
                NotificationOverlay.PushNotification(
                    new LocalisedString("Please note that this game is still in a very early alpha!"),
                    Color4.Yellow,
                    5000);
                
                NotificationOverlay.PushNotification(
                    new LocalisedString(
                        "Please consider reporting Every bug you find if it hasn't been found already in #bug-reports"),
                        Color4.Red, 
                    5000);
                
                NotificationOverlay.PushNotification(
                    new LocalisedString(
                        "Debug keys:\n" +
                        "    Arrow up: Speed up\n" +
                        "    Arrow down: Speed down\n" +
                        "    Space: Pause\n"),
                    Color4.Orange, 
                    5000);
            });
        }

        public override bool OnExiting(IScreen next)
        {
            this.FadeOutFromOne(2500, Easing.OutExpo);
            return true;
        }

        private StopwatchClock clock = new StopwatchClock();
        public bool IsFading;
        
        protected override void Update()
        {
            if (IsFading || clock.ElapsedMilliseconds <= 5000)
                return;
            
            Toolbar.FadeOut(13000);
            bottomBar.FadeOut(13000);
            
            IsFading = true;
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            Toolbar.ClearTransforms();
            bottomBar.ClearTransforms();
            
            Toolbar.FadeIn(250);
            bottomBar.FadeIn(250);
            
            IsFading = false;
            clock.Restart();
            return true;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (_qsorLogo.IsHovered)
                ((QsorGame) Game).PushScreen(new OsuScreen
                {
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    FillMode = FillMode.Fill,
                });
            
            return base.OnClick(e);
        }
    }
}