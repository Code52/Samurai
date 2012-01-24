using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public abstract class TileType
    {
        public string Name { get; set; }

        public string ImageSpriteResource { get; set; }

        public bool CanMoveOn { get; set; }
    }
}
