namespace Qsor.Game.Online.Users
{
    public class User
    {
        public int Id = 1;
        public string Username = "Guest";
        
        public UserStatistics Statistics { get; } = new UserStatistics();
    }
}