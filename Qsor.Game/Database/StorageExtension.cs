using osu.Framework.Platform;

namespace Qsor.Game.Database
{
    // Bring back storage stuff which was deleted from o!F some time ago
    internal static class StorageExtension
    {
        public static string GetDatabaseConnectionString(this Storage storage, string name)
            => string.Concat("Data Source=", storage.GetFullPath($@"{name}.db", true));

        public static  void DeleteDatabase(this Storage storage, string name)
            => storage.Delete($@"{name}.db");
    }
}
