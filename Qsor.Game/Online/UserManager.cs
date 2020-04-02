using Qsor.Game.Online.Users;

namespace Qsor.Game.Online
{
    public class UserManager
    {
        public User User { get; } = new User
        {
            Id = 0,
            Username = "Guest"
        };
    }
}