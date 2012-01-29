using System.Data.Entity.Migrations;
using SamuraiServer.Data.Impl.Sql;

namespace SamuraiServer.Migrations
{
    internal sealed class SamuraiDbConfiguration : DbMigrationsConfiguration<SamuraiContext>
    {
        public SamuraiDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SamuraiContext context)
        {
            // TODO: seed data
#if DEBUG
            
#endif
        }
    }
}
