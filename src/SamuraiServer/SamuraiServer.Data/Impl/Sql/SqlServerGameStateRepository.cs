using System.Linq;

namespace SamuraiServer.Data.Impl.Sql
{
    public class SqlServerGameStateRepository : GenericRepository<SamuraiContext, GameState>, IGameStateRepository
    {
        // TODO: We may also store our json in SQL nvarchar(MAX) if that is easier for people

        public SqlServerGameStateRepository(SamuraiContext ctx) 
            : base(ctx)
        {
        }

        public GameState GetByName(string name)
        {
            return base.FindBy(c => c.Name == name).FirstOrDefault();
        }
    }
}
