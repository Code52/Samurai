using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class Move
    {
        public Player Player { get; set; }

        public List<Unit> PlayersUnits { get; set; }

        public Dictionary<Player, List<Unit>> AffectedUnits { get; set; } 
    }
}
