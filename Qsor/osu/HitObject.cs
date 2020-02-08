using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osuTK;

namespace Qsor.osu
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
    
    public abstract class HitObject : Container, IHasEndTime
    {
        public double BeginTime;
        public virtual double EndTime => BeginTime + 500;

        public virtual double Duration => EndTime - BeginTime;
        public float HitObjectSize { get; }

        public ColourInfo HitObjectColour; // we do not use Colour, we use HitObjectColour instead, as Colour would Colour the whole HitCircle. (in theory, not tested)

        public abstract HitObjectType Type { get; }
        
        public TimingPoint TimingPoint { get; set; }
        
        public HitObject(Vector2 position, float size)
        {
            Position = position;
            
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;
            
            HitObjectSize = size;
        }
    }
}