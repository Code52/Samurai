using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class MoveSet
    {
        public Player Player { get; set; }
        public GameState Game { get; set; }

        public List<Move> Moves { get; set; } 
    }
}
