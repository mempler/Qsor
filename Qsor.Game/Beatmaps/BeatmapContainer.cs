using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using Qsor.Game.Gameplay;
using Qsor.Game.Gameplay.Containers;
using Qsor.Game.Graphics.Containers;

namespace Qsor.Game.Beatmaps
{
    public class BeatmapContainer : Container
    {
        private BackgroundImageContainer _background;
        public Bindable<WorkingBeatmap> WorkingBeatmap { get; } = new Bindable<WorkingBeatmap>();
        
        public PlayfieldAdjustmentContainer Playfield { get; private set; }
        
        [Resolved]
        private AudioManager Audio { get; set; }

        public BeatmapContainer(Bindable<WorkingBeatmap> beatmap)
        {
            WorkingBeatmap.BindTo(beatmap);
        }
        
        [BackgroundDependencyLoader]
        private void Load(RulesetManager rulesetManager, TextureStore store)
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            FillMode = FillMode.Fill;
            
            //LoadComponent(WorkingBeatmap.Value);

            AddInternal(_background = new BackgroundImageContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
            
            _background.SetTexture(WorkingBeatmap.Value.Background);
            Audio.AddItem(WorkingBeatmap.Value.Track);
            
            if (Playfield == null)
                AddInternal(Playfield = new PlayfieldAdjustmentContainer());

            var ruleset = rulesetManager.GetRuleset(0);
            Playfield.SetPlayfield(ruleset.CreatePlayfield());
        }
        
        public void PlayBeatmap()
        {
            WorkingBeatmap.Value.Track.Start();
        }
    }
}