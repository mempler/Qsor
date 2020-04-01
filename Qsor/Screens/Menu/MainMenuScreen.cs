using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Timing;
using Qsor.Beatmaps;
using Qsor.Database;
using Qsor.Gameplay.osu.Screens;
using Qsor.Graphics.Containers;

namespace Qsor.Screens.Menu
{
    public class MainMenuScreen : Screen
    {
        private BackgroundImageContainer _background;
        private QsorLogo _qsorLogo;
        private Toolbar Toolbar;

        private Bindable<WorkingBeatmap> WorkingBeatmap = new Bindable<WorkingBeatmap>();
        
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
                ParallaxAmount = -0.02f
            };
            parallaxFront._content.Add(_qsorLogo = new QsorLogo
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both
            });
            
            AddInternal(parallaxFront);

            
            
            AddInternal(Toolbar = new Toolbar());
        }
        
        protected override void LoadComplete()
        {
            WorkingBeatmap.Value.Play();
            
            base.LoadComplete();
        }
        
        public override void OnEntering(IScreen last)
        {
            clock.Start();
            this.FadeInFromZero(2500, Easing.InExpo);
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
            
            Toolbar.FadeOutFromOne(13000);
            
            IsFading = true;
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            Toolbar.ClearTransforms();
            
            Toolbar.FadeIn(250);
            
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