using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;

namespace Qsor.osu.HitObjects
{
    public enum HitSliderType
    {
        Linear,
        Perfect,
        Bezier,
        Catmull,
    }
    
    // TODO: Fully Implement.
    public class HitSlider : HitObject
    {
        
        private readonly double _pixelLength;
        private readonly int _repeats;

        public IReadOnlyList<Vector2> CurvePoints { get; }
        public HitSliderType SliderType { get; }
        
        public override HitObjectType Type => HitObjectType.Slider;
        
        private Path SliderPathInnerFront;
        private Path SliderPathInnerBack;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store) {
            SliderPathInnerFront = new TexturedPath
            {
                Anchor = Anchor.TopLeft,

                Texture = store.Get("slider"),
                
                Colour = HitObjectColour,
                PathRadius = 25
            };
            
            SliderPathInnerBack = new SmoothPath
            {
                Anchor = Anchor.TopLeft,
                
                Colour = Color4.Black,
                PathRadius = 20,
            };

            foreach (var curvePoint in CurvePoints)
            {
                SliderPathInnerFront.AddVertex(curvePoint);
                SliderPathInnerBack.AddVertex(curvePoint);
            }

            Anchor = Anchor.TopLeft;
            Origin = Anchor.TopLeft;
            
            Add(SliderPathInnerBack);
            Add(SliderPathInnerFront);
        }
        
        public HitSlider(HitSliderType sliderType, IReadOnlyList<Vector2> curvePoints,
                double pixelLength, int repeats,
            float size) : base(curvePoints[0], size)
        {
            SliderType = sliderType;
            CurvePoints = curvePoints;
            _pixelLength = pixelLength;
            _repeats = repeats;
        }
    }
}