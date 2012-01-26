using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    // This class is really just for JSON processing, we have a custom one here as we canot use the abstract one from the shared files
    public class TileType
    {
        public string Name { get; set; }

        public bool CanMoveOn { get; set; }

        public bool CanShootOver { get; set; }
    }
}