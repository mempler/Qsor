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
        private Box ProgressBox;
        
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
            
            AddInternal(new Box
            {
                Colour = new Color4(1f, 1f, 1f, 0.5f),
                RelativeSizeAxes = Axes.Both
            });
            
            AddInternal(ProgressBox = new Box
            {
                Colour = Color4.Yellow,
                RelativeSizeAxes = Axes.Y,
            });
            
            
        }

        protected override void LoadComplete()
        {
            UpdateProgress();
            base.LoadComplete();
        }

        public void UpdateProgress()
        {
            ProgressBox.Width = (int) (DrawSize.X * _user.Statistics.GetProgress());
        }
    }
}