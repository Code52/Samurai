using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class Player
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ApiKey { get; set; }

        public int Wins { get; set; }
        public int GamesPlayed { get; set; }

        public DateTime LastSeen { get; set; }

        public bool IsOnline { get; set; }

        public bool IsActive { get; set; }
    }
}
