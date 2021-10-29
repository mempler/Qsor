using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osuTK;
using Qsor.Game.Beatmaps;

namespace Qsor.Game.Gameplay
{
    [Flags]
    [SuppressMessage("ReSharper", "ShiftExpressionRealShiftCountIsZero")]
    public enum HitObjectType
    {
        Circle   = 1 << 0,
        Slider   = 1 << 1,
        NewCombo = 1 << 2,
        Spinner  = 1 << 3
    }
    
    public abstract class HitObject : Container
    {
        public double BeginTime;
        public virtual double EndTime => BeginTime + 300;

        public virtual double Duration => EndTime - BeginTime;
        public float HitObjectSize { get; }

        public ColourInfo HitObjectColour; // we do not use Colour, we use HitObjectColour instead, as Colour would Colour the whole HitCircle. (in theory, not tested)

        public abstract HitObjectType Type { get; }
        
        public readonly Bindable<int> StackHeightBindable = new();
        
        public Vector2 StackedPosition => Position + StackOffset;
        public Vector2 StackOffset => StackHeightBindable.Value * Scale * -6.4f;
        
        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }
        
        public Beatmap Beatmap { get; }

        public BindableDouble BindableScale = new();
        public BindableDouble BindableProgress = new();
        
        public HitObject(Beatmap beatmap, double beginTime, Vector2 position, Colour4 colour)
        {
            Position = position;
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;

            Beatmap = beatmap;

            HitObjectColour = colour;

            BeginTime = beginTime;
            
            BindableScale.Default = (1.0f - 0.7f * ((float) Beatmap.Difficulty.CircleSize - 5) / 5) / 2;
            BindableScale.SetDefault();

            BindableProgress.SetDefault();
            BindableProgress.Value = 0;
        }

        protected override void Update()
        {
            if (!(Beatmap is WorkingBeatmap workingBeatmap))
                return;
            
            BindableProgress.Value = Math.Clamp((workingBeatmap.Track.CurrentTime - BeginTime + Beatmap.General.AudioLeadIn) / Duration, 0, 1);
        }
    }
}