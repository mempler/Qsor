using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;
using Qsor.Beatmaps;
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
        
        public HitCircle SliderBeginCircle { get; private set; }
        public SnakingSliderBody Body { get; private set; }
        public SliderBall Ball { get; private set; }
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store) {
            Body = new SnakingSliderBody(this)
            {
                PathRadius = (1.0f - 0.7f * ((float) Beatmap.Difficulty.CircleSize - 5) / 5) / 2 * 64,
                AccentColour = Color4.Black
            }; // lets make it black for now, as almost every Legacy skin uses that.

            Body.SnakingIn.Value = false;
            Body.SnakingOut.Value = false;
            
            Origin = Anchor.TopLeft;

            Position = StackedPosition;

            Ball = new SliderBall
            {
                Scale = new Vector2(Body.PathRadius),
                Origin = Anchor.Centre
            };
            
            SliderBeginCircle = new HitCircle(Beatmap, Body.PathOffset) {BeginTime = BeginTime};
            
            InternalChildren = new Drawable[]
            {
                Body,
                Ball,
                SliderBeginCircle
            };
            
            Body.UpdateProgress(0);
            Ball.Position = Path.PositionAt(0);
            Ball.Scale = new Vector2(Body.PathRadius / 64f);

            BindableProgress.ValueChanged += prog =>
            {
                if (prog.NewValue * this.SpanCount() >= 1)
                    Hide();
                
                Body.UpdateProgress(prog.NewValue);
                Ball.Position = Path.PositionAt(prog.NewValue * this.SpanCount());
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
        }

        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }
        
        protected override void Update()
        {
            Size = Body.Size;
            OriginPosition = Body.PathOffset;
            
            base.Update();
        }
    }
}