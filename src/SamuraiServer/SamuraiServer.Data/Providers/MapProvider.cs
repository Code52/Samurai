using SamuraiServer.Data.Tiles;
namespace SamuraiServer.Data.Providers
{
    public class MapProvider : IMapProvider
    {
        public Map GetRandomMap() {
            // Replace with random generation / importing from a string in format
            // """"~"""""
            // """"~"""""
            // """"~@@"""
            // """~""""""
            // ""~""""""T
            // "~"@""""""
            // ~""""""T""
            var map = new Map {
                Tiles = new[] {
                    new TileType[] { G(), G(), G(), W(), G(), G(), G(), G(), G(), G()},
                    new TileType[] { G(), G(), G(), W(), G(), G(), G(), G(), G(), G()},
                    new TileType[] { G(), G(), W(), W(), G(), G(), G(), G(), G(), G()},
                    new TileType[] { W(), W(), W(), G(), R(), G(), G(), G(), G(), G()},
                    new TileType[] { G(), G(), G(), G(), G(), G(), G(), G(), G(), G()},
                    new TileType[] { G(), G(), G(), G(), G(), G(), R(), G(), G(), G()},
                    new TileType[] { G(), G(), R(), G(), G(), G(), G(), G(), G(), G()},
                    new TileType[] { G(), G(), G(), G(), G(), G(), T(), G(), G(), G()},
                    new TileType[] { G(), G(), G(), G(), G(), G(), T(), G(), G(), G()},
                    new TileType[] { G(), G(), G(), G(), G(), G(), T(), G(), G(), G()}
                }
            };
            return map;
        }
        public static Grass G() { return new Grass(); }

        public static Rock R() { return new Rock(); }

        public static Tree T() { return new Tree(); }

        public static Water W() { return new Water(); }
    }
}