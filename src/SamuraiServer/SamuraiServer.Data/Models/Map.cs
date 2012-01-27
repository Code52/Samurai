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

        public static Map FromStringRepresentation(Guid id, string[] rows)
        {
            Map m = new Map();
            m.Id = id;
            m.Tiles = new TileType[rows[0].Length][];
            for (int x = 0; x < m.Tiles.Length; x++)
            {
                m.Tiles[x] = new TileType[rows.Length];
                for (int y = 0; y < rows.Length; y++)
                {
                    m.Tiles[x][y] = new TileType();
                    m.Tiles[x][y].StringRepresentation = rows[y][x];
                }
            }
            return m;
        }
    }
}
