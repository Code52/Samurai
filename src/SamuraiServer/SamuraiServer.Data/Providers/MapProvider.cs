using System;
using System.Collections.Generic;
using SamuraiServer.Data.Tiles;
namespace SamuraiServer.Data.Providers
{
    public class MapProvider : IMapProvider
    {
        private static Dictionary<Guid, Map> _maps = new Dictionary<Guid, Map>();

        static MapProvider()
        {
            // Replace with random generation / importing from a string in format
            // """"~"""""
            // """"~"""""
            // """"~@@"""
            // """~""""""
            // ""~""""""T
            // "~"@""""""
            // ~""""""T""
            var map = new Map
            {
                Tiles = new[] {
                    new TileType[] { R(), R(), G(), G(), G(), G(), G(), G(), G(), G()},
                    new TileType[] { R(), G(), G(), W(), G(), G(), G(), T(), G(), G()},
                    new TileType[] { R(), G(), W(), W(), G(), T(), R(), T(), G(), R()},
                    new TileType[] { G(), G(), W(), G(), G(), T(), T(), R(), G(), G()},
                    new TileType[] { G(), G(), W(), T(), T(), G(), R(), R(), G(), G()},
                    new TileType[] { G(), G(), W(), T(), G(), G(), R(), G(), G(), W()},
                    new TileType[] { G(), G(), G(), G(), G(), G(), G(), G(), W(), W()},
                },
                MinPlayers = 2,
                MaxPlayers = 2,
                StartingUnits = new Dictionary<int, List<Unit>>()
                  { 
                    { 0, new List<Unit>
                        {
                            new Samurai() { X = 0, Y = 6 },
                            new Samurai() { X = 1, Y = 6 },
                            new Samurai() { X = 0, Y = 5 }
                        }
                    },
                    { 1, new List<Unit>
                        {
                            new Samurai() { X = 9, Y = 0 },
                            new Samurai() { X = 8, Y = 0 },
                            new Samurai() { X = 9, Y = 1 }
                        }
                    }
                }  
            };


            _maps.Add(map.Id, map);
        }

        public Map GetRandomMap()
        {
            return _maps.RandomElement().Value;
        }

        public Map Get(Guid id)
        {
            return _maps[id];
        }

        public static Grass G() { return new Grass(); }

        public static Rock R() { return new Rock(); }

        public static Tree T() { return new Tree(); }

        public static Water W() { return new Water(); }
    }
}