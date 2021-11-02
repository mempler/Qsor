using Microsoft.EntityFrameworkCore;
using osu.Framework.Logging;
using Qsor.Game.Database.Models;

namespace Qsor.Game.Database
{
    public sealed class QsorDbContext : DbContext
    {
        private readonly string _connectionString;
        
        public QsorDbContext()
            : this("DataSource=:memory:")
        {
        }
        
        public QsorDbContext(string connectionString)
        {
            _connectionString = connectionString;

            var connection = Database.GetDbConnection();

            try
            {
                connection.Open();

                using var cmd = connection.CreateCommand();
                
                cmd.CommandText = "PRAGMA journal_mode=WAL;";
                cmd.ExecuteNonQuery();
            }
            catch
            {
                connection.Close();
                throw;
            }
        }
        
        public DbSet<BeatmapModel> Beatmaps { get; set; }

        public void Migrate()
        {
            Logger.LogPrint("Migrating Database");
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}