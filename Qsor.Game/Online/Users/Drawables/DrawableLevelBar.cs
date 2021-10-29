using osu.Framework.Allocation;
using osuTK.Graphics;
using Qsor.Game.Overlays.Drawables;

namespace Qsor.Game.Online.Users.Drawables
{
    public class DrawableLevelBar : DrawableProgressbar
    {
        private readonly User _user;

        public DrawableLevelBar(User user)
        {
            _user = user;
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Colour = Color4.Yellow;
        }

        protected override void LoadComplete()
        {
            Current.Value = _user.Statistics.GetProgress();
        }
    }
}