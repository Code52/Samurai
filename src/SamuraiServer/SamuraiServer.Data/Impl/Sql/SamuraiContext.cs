using System.Data.Entity;

namespace SamuraiServer.Data.Impl.Sql
{
    public class SamuraiContext : DbContext
    {
        public SamuraiContext()
            : base("Samurai")
        {
            Configuration.ProxyCreationEnabled = false;
        }

    }
}
