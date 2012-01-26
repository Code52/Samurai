using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Tiles
{
    public class Rock : TileType
    {
        public override string Name { get { return "Rock"; } }

        public override bool CanMoveOn { get { return false; } }

        public override bool CanShootOver { get { return true; } }
    }
}