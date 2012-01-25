using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class GamePlayer
    {
        public Guid Id { get; set; }

        public Player Player { get; set; }

        public List<Unit> Units { get; set; }

        public int Score { get; set; }

        public bool IsAlive { get; set; }
    }
}
