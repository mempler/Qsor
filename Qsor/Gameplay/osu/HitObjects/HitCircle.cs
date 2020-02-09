using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Qsor.Gameplay.osu.HitObjects
{
    public class HitCircle : HitObject
    {
        private Sprite _hitCircle;
        private Sprite _approachCircle;
        private Sprite _hitCircleOverlay;

        public override HitObjectType Type => HitObjectType.Circle;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            _hitCircle = new Sprite
            {
                Texture = store.Get("hitcircle.png"),
                
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = HitObjectColour,
                
                Size = new Vector2(64, 64),
            };

            _approachCircle = new Sprite
            {
                Texture = store.Get("approachcircle.png"),
                
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                
                Scale = new Vector2(2),
                
                Size = new Vector2(128, 128),
            };

            _hitCircleOverlay = new Sprite
            {
                Texture = store.Get("hitcircleoverlay.png"),
                
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                
                Size = new Vector2(64, 64)
            };

            Add(_hitCircle);
            Add(_approachCircle);
            Add(_hitCircleOverlay);

            Alpha = 0;
            Size = new Vector2(128, 128) * HitObjectSize;
        }

        public override void Hide()
        {
            //base.Hide();
            this.FadeTo(0, 100)
                .Finally(o =>
            {
                (Parent as Container)?.Remove(this); // TODO: Fix
            });
            
            _approachCircle.FadeTo(0, 100);
            _hitCircleOverlay.ScaleTo(new Vector2(2f), 100);
            //_hitCircleOverlay.MoveToOffset(new Vector2(0, 1), 200);
        }

        public override void Show()
        {
            //base.Show();

            this.FadeTo(1, 200);
            _approachCircle.ScaleTo(0, Duration + (Duration * .3));
        }

        public HitCircle(Vector2 position, float size) : base(position, size)
        {
        }
    }
}