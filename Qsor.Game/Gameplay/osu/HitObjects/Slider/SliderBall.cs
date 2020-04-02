using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace Qsor.Game.Gameplay.osu.HitObjects.Slider
{
    public class SliderBall : Container
    {
        private Sprite _ball;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            var ballTex = store.Get("sliderb");
            
            Origin = Anchor.Centre;
            
            Blending = BlendingParameters.Additive;
            
            AddInternal(_ball = new Sprite
            {
                Texture = ballTex,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
            
            Size = _ball.Size;
        }
    }
}