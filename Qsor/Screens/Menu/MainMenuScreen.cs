using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using Qsor.Beatmaps;
using Qsor.Database;
using Qsor.Graphics.Containers;
using Qsor.Overlays;

namespace Qsor.Screens
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
            
            AddInternal(_background = new BackgroundImageContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
            
            var db = ctxFactory.Get();
            var beatmapModel = db.Beatmaps.FirstOrDefault();
            var beatmapStorage = storage.GetStorageForDirectory(beatmapModel?.Path);
            beatmapManager.LoadBeatmap(beatmapStorage, beatmapModel?.File);
            LoadComponent(beatmapManager.WorkingBeatmap.Value);
            WorkingBeatmap.BindTo(beatmapManager.WorkingBeatmap);
            
            _background.SetTexture(WorkingBeatmap.Value.Background);
            
            audioManager.AddItem(WorkingBeatmap.Value.Track);
            
            AddInternal(_qsorLogo = new QsorLogoOverlay
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });

            AddInternal(_toolBarTop);
        }
        
        protected override void LoadComplete()
        {
            WorkingBeatmap.Value.Play();
            
            base.LoadComplete();
        }
        
        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(5000, Easing.InCubic);
        }
    }
}