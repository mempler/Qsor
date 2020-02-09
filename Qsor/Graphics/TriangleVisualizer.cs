using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Logging;
using osu.Framework.Physics;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Graphics
{
    public class VisualizedTriangle : Triangle
    {
        public double SpeedBoost { get; set; } = .5;
        public double SpeedMultiply { get; set; }
        public Vector2 OriginalScale { get; internal set; }
        
        protected override void Update()
        { 
            if (Parent == null)
                return;

            if (Position.Y > Parent.DrawHeight)
                (Parent as Container)?.Remove(this);

            this.MoveToY((float) (Position.Y + ((SpeedMultiply * SpeedBoost)) * Clock.ElapsedFrameTime));
        }
        
        public TransformSequence<VisualizedTriangle> SpeedBoostTo(double newSpeedBoost, double duration = 0, Easing easing = Easing.None) =>
            this.TransformTo(nameof(SpeedBoost), newSpeedBoost, duration, easing);
    }
    
    
    public class TriangleVisualizer : Container
    {
        private Random _rand = new Random();
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            SpawnTriangle();
        }

        public void SpawnTriangle()
        {
            var colMul = (float) Math.Max(.4, _rand.NextDouble());

            var tri = new VisualizedTriangle
            {
                Size = new Vector2(100, 100),
                Scale = new Vector2((float) Math.Max(.4, _rand.NextDouble())),
                
                Origin = Anchor.Centre,

                Position = new Vector2(_rand.Next(0, (int) DrawWidth), -DrawWidth),
                
                SpeedMultiply = Math.Min(_rand.NextDouble(), .8f),
                Colour = Color4.White.Multiply(colMul)
            };

            tri.OriginalScale = tri.Scale;
            
            Add(tri);
        }

        public void SpawnTriangle(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                SpawnTriangle();
            }
        }

        public IEnumerable<TransformSequence<VisualizedTriangle>> ScaleTriangles(float scale, double duration = 0d, Easing easing = Easing.None)
        {
            return
                from child in Children
                where (VisualizedTriangle) child != null
                select ((VisualizedTriangle) child).ScaleTo(((VisualizedTriangle) child).OriginalScale * scale, duration, easing);
        }
        
        public IEnumerable<TransformSequence<VisualizedTriangle>> SpeedBoostTriangles(double boost, double duration = 0d, Easing easing = Easing.None)
        {
            return
                from child in Children
                where (VisualizedTriangle) child != null
                select ((VisualizedTriangle) child).SpeedBoostTo(boost, duration, easing);
        }

        public void SpeedBoost(double amount, double duration)
        {
            foreach (var child in Children)
            {
                var tri = (VisualizedTriangle) child;
                if (tri == null)
                    continue;
                
                tri.SpeedBoost = amount;
            }
        }
    }
}