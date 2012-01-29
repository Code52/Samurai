using System;
using System.Collections.Generic;
using System.Linq;
using SamuraiServer.Data.Tiles;

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

        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public Dictionary<int, List<Unit>> StartingUnits { get; set; }

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
                    TileType t;
                    switch (rows[y][x])
                    {

                        case '.':
                            t = new Grass();
                            break;

                        case '@':
                            t = new Rock();
                            break;

                        case 'T':
                            t = new Tree();
                            break;

                        case '~':
                            t = new Water();
                            break;

                        default:
                            continue;
                    }
                    m.Tiles[x][y] = t;
                }
            }
            return m;
        }
    }
}
