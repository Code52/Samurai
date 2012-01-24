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

        public List<Player> Players { get; set; }

        public GameState() {
            Players = new List<Player>();
        }
    }
}
