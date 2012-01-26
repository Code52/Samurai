using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Tiles
{
    public class Water : TileType
    {
        public override string Name { get { return "Water"; } }

        public override bool CanMoveOn { get { return false; } }

        public override bool CanShootOver { get { return true; } }

        public override string StringRepresentation
        {
            get { return "~"; }
        }
    }
}