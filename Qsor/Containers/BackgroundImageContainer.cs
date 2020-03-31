using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Containers
{
    public class BackgroundImageContainer : BufferedContainer
    {
        private Sprite _backgroundImage;

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.Both;
            
            Child = _backgroundImage = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };
            
            CacheDrawnFrameBuffer = true;

            BackgroundColour = Color4.Black;
            Colour = new Color4(1,1,1, .5f);
            BlurSigma = new Vector2(5f);
        }

        public void SetTexture(Texture tex)
        {
            _backgroundImage.Texture = tex;
            
            ForceRedraw();
        }
    }
}