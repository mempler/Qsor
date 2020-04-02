namespace Qsor.Game.Online.Users
{
    public class UserStatistics
    {
        public int PerformancePoints;
        public double Accuracy;

        public int Rank;

        public int GetLevel() => 0;
        public double GetProgress() => .5d;
    }
}