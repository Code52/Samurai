using System;
using System.Collections.Generic;

namespace SamuraiServer.Data
{
    public class GameState
    {
        public Guid Id { get; set; }

        public string Name { get; set; }  // NOTE: what does this represent? <csainty: Lets the user name a game so they can tell a friend which to join. Could use invites instead>

        public List<GamePlayer> Players { get; set; }

        public int Turn { get; set; }

        public bool Started { get; set; }

        public Guid MapId { get; set; }

        public List<Guid> PlayerOrder { get; set; } 

        public GameState()
        {
            Players = new List<GamePlayer>();
            PlayerOrder = new List<Guid>();
        }
    }
}
