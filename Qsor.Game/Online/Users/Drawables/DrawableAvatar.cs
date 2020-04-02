using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Qsor.Game.Online.Users.Drawables
{
    public class DrawableAvatar : CompositeDrawable
    {
        private Sprite Avatar { get; } = new Sprite();

        public Bindable<User> User = new Bindable<User>(new User{ Id = 1 });

        [BackgroundDependencyLoader]
        private void Load(TextureStore ts)
        {
            User.ValueChanged += async e
                => Avatar.Texture = await ts.GetAsync(BanchoClient.AVATAR_URL + "/" + (e.NewValue?.Id ?? User.Default.Id));

            Avatar.FillMode = FillMode.Fit;
            Avatar.RelativeSizeAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            
            Margin = new MarginPadding(10);
            Scale = new Vector2(.8f);

            AddInternal(Avatar);
        }
    }
}