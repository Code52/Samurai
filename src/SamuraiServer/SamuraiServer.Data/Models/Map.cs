using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class Map
    {
        public Map()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string ImageResource { get; set; }

        public TileType[][] Tiles { get; set; }

        public string[] GetStringRepresentation()
        {
            return Tiles.Select(t => new string(t.Select(t2 => t2.StringRepresentation).ToArray())).ToArray();
        }
    }
}
