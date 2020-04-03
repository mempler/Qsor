using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using Qsor.Game.Online.Users;

namespace Qsor.Game.Overlays.Drawables
{
    public class DrawableLevelBar : Container
    {
        private Box _progressBox;
        
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
            
            AddInternal(_progressBox = new Box
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
            _progressBox.Width = (int) (DrawSize.X * _user.Statistics.GetProgress());
        }
    }
}