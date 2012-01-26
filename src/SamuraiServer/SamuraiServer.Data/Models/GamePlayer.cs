using System;
using System.Collections.Generic;

namespace SamuraiServer.Data
{
    public class GamePlayer
    {
        public GamePlayer()
        {
            Units = new List<Unit>();
        }

        public Guid Id { get; set; }

        public Player Player { get; set; }

        public IList<Unit> Units { get; set; }

        public int Score { get; set; }

        public bool IsAlive { get; set; }
    }
}
