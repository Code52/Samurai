using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public interface IPlayerRepository: IGenericRepository<Player, Guid>
    {
    }
}
