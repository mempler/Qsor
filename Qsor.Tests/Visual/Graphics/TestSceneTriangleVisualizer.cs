using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK.Graphics;
using Qsor.Graphics;

namespace Qsor.Tests.Visual.Graphics
{
    public class TestSceneTriangleVisualizer : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(TriangleVisualizer), typeof(TriangleVisualizer) };

        private TriangleVisualizer TriangleVisualizer;
        private Track _track;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audioManager)
        {
            TriangleVisualizer = new TriangleVisualizer
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.YellowGreen
            };
            Add(TriangleVisualizer);

            _track = audioManager.Tracks.Get("WATEVA - Ber Zer Ker (Rob Gasser Remix) [NCS Release]");
            
            AddUntilStep("Wait for Load", () => TriangleVisualizer.IsLoaded && _track.IsLoaded);
            
            AddStep("Spawn Triangle", TriangleVisualizer.SpawnTriangle);
            AddStep("Spawn 50 Triangle", () => TriangleVisualizer.SpawnTriangle(50));
            AddStep("Spawn 100 Triangle", () => TriangleVisualizer.SpawnTriangle(100));
            AddStep("Spawn 200 Triangle", () => TriangleVisualizer.SpawnTriangle(200));
            AddStep("Spawn 400 Triangle", () => TriangleVisualizer.SpawnTriangle(400));
            AddStep("Spawn 800 Triangle", () => TriangleVisualizer.SpawnTriangle(800));

            AddStep("Random Colour", TriangleVisualizer.RandomColour);
            
            AddToggleStep("Music", e =>
            {
                if (e)
                    _track.Start();
                else
                    _track.Stop();
            });
            
            AddSliderStep("Track Volume", 0, 100, 20, d => _track.Volume.Value = d / 100f);
            AddSliderStep("Track Frequency", 5, 200, 100, d => _track.Frequency.Value = d / 100f);
            AddSliderStep("Track Tempo", 5, 100, 100, d => _track.Tempo.Value = d / 100f);
        }
        
        protected override void Update()
        {
            if (_track?.CurrentAmplitudes.FrequencyAmplitudes == null)
                return;
            
            var higherFreq = _track?.CurrentAmplitudes.FrequencyAmplitudes.TakeLast(50);
            var lowerFreq = _track?.CurrentAmplitudes.FrequencyAmplitudes.TakeLast(100);

            var avg = lowerFreq?.Sum() + higherFreq?.Sum() ?? 0;
            
            if (avg >= .8) {
                TriangleVisualizer?.RandomColour();
                TriangleVisualizer?.FlashColour(Color4.White, 100);
                TriangleVisualizer?
                    .ScaleTriangles(1.1f, 100)
                    .ForEach(s => s.Then(e => e.ScaleTo(e.OriginalScale, 100)));

                TriangleVisualizer?
                    .SpeedBoostTriangles(avg * 3, 100)
                    .ForEach(s => s.Then(e => e.SpeedBoostTo(.5, 100)));
                
                TriangleVisualizer?.SpawnTriangle(1);
            }
            
            TriangleVisualizer?
                .ScaleTriangles(avg * 2.5f, 500)
                .ForEach(s => s.Then(e => e.ScaleTo(e.OriginalScale, 500)));
            
        }
    }
}