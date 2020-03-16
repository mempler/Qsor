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
            Body = new SnakingSliderBody(this) {PathRadius = ((1.0f - 0.7f * ((float) Beatmap.Difficulty.CircleSize - 5) / 5) / 2) * 64, AccentColour = Color4.Black}; // lets make it black for now, as almost every Legacy skin uses that.

            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;
            
            Add(Body);

            SliderBeginCircle.Position = Body.PathOffset;
            Add(SliderBeginCircle);

            BindableProgress.ValueChanged += prog =>
            {
                if (prog.NewValue >= .5)
                    ((Container) Parent)?.Remove(this);
            
                Body.UpdateProgress(prog.NewValue);
            };
        }

        public override void Show()
        {
            SliderBeginCircle.Show();
            //this.FadeInFromZero(200 + Beatmap.Difficulty.ApproachRate);
        }

        public override void Hide()
        {
            SliderBeginCircle.Hide();
            this.FadeOutFromOne(200 + Beatmap.Difficulty.ApproachRate);
        }
        
        public HitSlider(Beatmap beatmap, PathType pathType, IReadOnlyList<Vector2> controlPoints,
                double pixelLength, int repeats) : base(beatmap, Vector2.Zero)
        {
            Path = new SliderPath(pathType, controlPoints.ToArray(), pixelLength);
            PathType = pathType;
            ControlPoints = controlPoints;

            RepeatCount = repeats;
            
            SliderBeginCircle = new HitCircle(beatmap, controlPoints[0]);
            SliderBeginCircle.BeginTime = BeginTime;
            SliderBeginCircle.AutoHide = false;
        }
    }
}