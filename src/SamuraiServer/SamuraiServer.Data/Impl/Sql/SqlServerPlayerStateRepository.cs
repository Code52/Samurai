using System.Linq;

namespace SamuraiServer.Data.Impl.Sql
{
    public class SqlServerPlayerRepository : GenericRepository<SamuraiContext, Player>, IPlayerRepository
    {
        public SqlServerPlayerRepository(SamuraiContext ctx) : base(ctx)
        {
        }

        public Player GetByName(string name)
        {
            return FindBy(p => p.Name == name).FirstOrDefault();
        }
    }
}
