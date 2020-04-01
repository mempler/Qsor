using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK.Graphics;
using Qsor.Beatmaps;
using Qsor.Database;
using Qsor.Graphics.Containers;
using Qsor.Overlays;

namespace Qsor.Screens.Menu
{
    public class MainMenuScreen : Screen
    {
        private Container _toolBarTop;
        private BackgroundImageContainer _background;
        private QsorLogoOverlay _qsorLogo;

        private Bindable<WorkingBeatmap> WorkingBeatmap = new Bindable<WorkingBeatmap>();
        
        [BackgroundDependencyLoader]
        private void Load(AudioManager audioManager, Storage storage, QsorDbContextFactory ctxFactory, BeatmapManager beatmapManager)
        {
            _toolBarTop = new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 80
            };
            
            _toolBarTop.Add(new Box
            {
                RelativeSizeAxes =  Axes.Both, 
                Colour = Color4.Black,
                Alpha = .4f
            });
            _toolBarTop.Add(new UserOverlay());
            
            var parallaxBack = new ParallaxContainer
            {
                ParallaxAmount = 0.005f
            };
            parallaxBack._content.Add(_background = new BackgroundImageContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
            AddInternal(parallaxBack);
            
            var db = ctxFactory.Get();
            var beatmapModel = db.Beatmaps.FirstOrDefault();
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
            parallaxFront._content.Add(_qsorLogo = new QsorLogoOverlay
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
            AddInternal(parallaxFront);

            AddInternal(_toolBarTop);
        }
        
        protected override void LoadComplete()
        {
            WorkingBeatmap.Value.Play();
            
            base.LoadComplete();
        }
        
        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(2500, Easing.InExpo);
        }
    }
}