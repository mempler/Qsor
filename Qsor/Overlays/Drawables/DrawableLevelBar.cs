using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using Qsor.Online.Users;

namespace Qsor.Overlays.Drawables
{
    public class DrawableLevelBar : BufferedContainer
    {
        private readonly User _user;

        public DrawableLevelBar(User user)
        {
            _user = user;
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            Height = 6;

            Masking = true;

            CornerRadius = 3;
            CornerExponent = 2f;

            var progressBox = new Box
            {
                Colour = Color4.Yellow,
                RelativeSizeAxes = Axes.Y,
                Width = (int) (135 * _user.Statistics.GetProgress())
            };
            
            AddInternal(new Box
            {
                Colour = new Color4(1f, 1f, 1f, 0.5f),
                RelativeSizeAxes = Axes.Both
            });
            
            AddInternal(progressBox);
        }
    }
}