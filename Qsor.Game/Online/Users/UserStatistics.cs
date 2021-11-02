namespace Qsor.Game.Online.Users
{
    public class UserStatistics
    {
        public int PerformancePoints = 0;
        public double Accuracy = 0;

        public int Rank = 0;

        public int GetLevel() => 0;
        public double GetProgress() => .5d;
    }
}