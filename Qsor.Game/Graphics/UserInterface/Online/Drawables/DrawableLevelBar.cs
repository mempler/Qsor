using osu.Framework.Allocation;
using osuTK.Graphics;
using Qsor.Game.Graphics.Drawables;
using Qsor.Game.Online.Users;

namespace Qsor.Game.Graphics.UserInterface.Online.Drawables
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