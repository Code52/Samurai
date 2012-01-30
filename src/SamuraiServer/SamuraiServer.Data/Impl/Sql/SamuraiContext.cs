using System.Data.Entity;

namespace SamuraiServer.Data.Impl.Sql
{
    public class SamuraiContext : DbContext
    {
        public SamuraiContext() : base("Samurai")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<GameState> Games { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
