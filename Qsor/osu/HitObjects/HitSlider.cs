﻿using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;

namespace Qsor.osu.HitObjects
{
    // TODO: Fully Implement.
    public class HitSlider : HitObject
    {
        public IReadOnlyList<Vector2> CurvePoints { get; private set; }
        public IReadOnlyList<Vector2> ControlPoints { get; }
        public PathType PathType { get; }
        
        public override HitObjectType Type => HitObjectType.Slider;
        
        private Path SliderPathInnerFront;
        private Path SliderPathInnerBack;

        private SliderPath _sliderPath;
        
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
            
            var curvePoints = new List<Vector2>();
            _sliderPath.GetPathToProgress(curvePoints, 0, 1);

            CurvePoints = curvePoints;
            foreach (var curvePoint in curvePoints)
            {
                SliderPathInnerFront.AddVertex(curvePoint);
                SliderPathInnerBack.AddVertex(curvePoint);
            }

            Anchor = Anchor.TopLeft;
            Origin = Anchor.TopLeft;
            
            Add(SliderPathInnerBack);
            Add(SliderPathInnerFront);
        }
        
        public HitSlider(PathType pathType, IReadOnlyList<Vector2> controlPoints,
                double pixelLength, int repeats,
            float size) : base(new Vector2(0,0), size)
        {
            _sliderPath = new SliderPath(pathType, controlPoints.ToArray(), pixelLength);
            PathType = pathType;
            ControlPoints = controlPoints;
        }
    }
}