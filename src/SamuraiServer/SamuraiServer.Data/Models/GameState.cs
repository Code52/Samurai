using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class GameState
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<GamePlayer> Players { get; set; }

        public int Turn { get; set; }

        public bool Started { get; set; }

        public Map Map { get; set; }

        public Dictionary<int, Guid> PlayerOrder { get; set; } 

        public GameState()
        {
            Players = new List<GamePlayer>();
        }
    }
}
