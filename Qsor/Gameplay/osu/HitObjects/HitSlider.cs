using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;
using Qsor.Gameplay.osu.HitObjects.Slider;

namespace Qsor.Gameplay.osu.HitObjects
{
    // TODO: Fully Implement.
    public class HitSlider : HitObject, IHasCurve
    {
        public override double EndTime => BeginTime + (Path.Distance * this.SpanCount()) / TimingPoint.Velocity;
        public IReadOnlyList<Vector2> ControlPoints { get; }
        public PathType PathType { get; }
        public override HitObjectType Type => HitObjectType.Slider;
        public SliderPath Path { get; }
        public int RepeatCount { get; set; }
        
        private SnakingSliderBody _body;
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store) {
            _body = new SnakingSliderBody(this) {PathRadius = 25, AccentColour = Color4.Black}; // lets make it black for now, as almost every Legacy skin uses that.

            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;
            
            Add(_body);
        }

        public override void Show()
        {
            this.FadeInFromZero(200 + Beatmap.Difficulty.ApproachRate);
        }

        public override void Hide()
        {
            this.FadeOutFromOne(200 + Beatmap.Difficulty.ApproachRate);
        }

        protected override void Update()
        {
            base.Update();
            
            _body.UpdateProgress(1);
        }

        public HitSlider(PathType pathType, IReadOnlyList<Vector2> controlPoints,
                double pixelLength, int repeats,
            float size) : base(new Vector2(0,0), size)
        {
            Path = new SliderPath(pathType, controlPoints.ToArray(), pixelLength);
            PathType = pathType;
            ControlPoints = controlPoints;

            RepeatCount = repeats;
        }
    }
}