using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class Map
    {
        public string Name { get; set; }
        public string ImageResource { get; set; }

        public TileType[][] Tiles { get; set; }
    }
}
