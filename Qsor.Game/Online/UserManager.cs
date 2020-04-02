using Qsor.Online.Users;

namespace Qsor.Online
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