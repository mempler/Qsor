using System.Threading;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using Qsor.Game.Online;
using Qsor.Game.Online.Users;

namespace Qsor.Game.Graphics.UserInterface.Online.Drawables
{
    public partial class DrawableAvatar : CompositeDrawable
    {
        private Sprite Avatar { get; } = new();

        public Bindable<User> User = new(new User{ Id = 1 });

        private CancellationTokenSource _cancellationTokenSource = new();

        [BackgroundDependencyLoader]
        private void Load(TextureStore ts)
        {
            User.ValueChanged += async e
                => Avatar.Texture = await ts.GetAsync(BanchoClient.AvatarUrl + "/" + (e.NewValue?.Id ?? User.Default.Id), _cancellationTokenSource.Token);

            Avatar.FillMode = FillMode.Fit;
            Avatar.RelativeSizeAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            
            Margin = new MarginPadding(10);
            Scale = new Vector2(.8f);

            AddInternal(Avatar);
        }
    }
}