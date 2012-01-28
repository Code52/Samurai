using System;
using System.Collections.Generic;

namespace SamuraiServer.Data
{
    public class GameState
    {
        public Guid Id { get; set; }

        public string Name { get; set; }  // NOTE: what does this represent?

        public List<GamePlayer> Players { get; set; }

        public int Turn { get; set; }

        public bool Started { get; set; }

        public Guid MapId { get; set; }

        public Dictionary<int, Guid> PlayerOrder { get; set; } 

        public GameState()
        {
            Players = new List<GamePlayer>();
        }
    }
}
