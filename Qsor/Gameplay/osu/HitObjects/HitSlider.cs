using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
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
        
        public HitCircle SliderBeginCircle { get; }
        public SnakingSliderBody Body { get; private set; }
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store) {
            Body = new SnakingSliderBody(this)
            {
                PathRadius = (1.0f - 0.7f * ((float) Beatmap.Difficulty.CircleSize - 5) / 5) / 2 * 64,
                AccentColour = Color4.Black
            }; // lets make it black for now, as almost every Legacy skin uses that.

            Body.SnakingIn.Value = false;
            Body.SnakingOut.Value = true;
            
            SliderBeginCircle.Position = Body.PathOffset;

            Origin = Anchor.TopLeft;

            Position = StackedPosition;
            
            Body.UpdateProgress(0);

            BindableProgress.ValueChanged += prog =>
            {
                if (prog.NewValue >= .5)
                    Hide();
                
                Body.UpdateProgress(prog.NewValue);
            };

            InternalChildren = new Drawable[]
            {
                Body,
                SliderBeginCircle
            };
        }

        public override void Show()
        {
            if (_isFading)
                return;
            
            this.FadeInFromZero(200 + Beatmap.Difficulty.ApproachRate);
            SliderBeginCircle.Show();
        }
        
        private bool _isFading;
        public override void Hide()
        {
            if (_isFading)
                return;
            
            _isFading = true;
            this.FadeOutFromOne(200 + Beatmap.Difficulty.ApproachRate).Finally(_ => ((Container) Parent)?.Remove(this));
            SliderBeginCircle.Hide();
        }
        
        public HitSlider(Beatmap beatmap, PathType pathType, IReadOnlyList<Vector2> controlPoints,
                double pixelLength, int repeats) : base(beatmap, controlPoints[0])
        {
            Path = new SliderPath(pathType, controlPoints.ToArray(), pixelLength);
            PathType = pathType;
            ControlPoints = controlPoints;

            RepeatCount = repeats;
            
            SliderBeginCircle = new HitCircle(beatmap, controlPoints[0]);
            SliderBeginCircle.BeginTime = BeginTime;
        }

        protected override void Update()
        {
            base.Update();

            Size = Body.Size;
            OriginPosition = Body.PathOffset;
        }
    }
}