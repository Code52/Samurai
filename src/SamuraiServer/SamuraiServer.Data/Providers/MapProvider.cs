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
                    new TileType[] { TileType.G(), TileType.G(), TileType.G(), TileType.W(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.G(), TileType.G(), TileType.G(), TileType.W(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.G(), TileType.G(), TileType.W(), TileType.W(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.W(), TileType.W(), TileType.W(), TileType.G(), TileType.R(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.R(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.G(), TileType.G(), TileType.R(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.T(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.T(), TileType.G(), TileType.G(), TileType.G()},
                    new TileType[] { TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.G(), TileType.T(), TileType.G(), TileType.G(), TileType.G()}
                }
            };
            return map;
        }
    }
}