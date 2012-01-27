using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Tiles
{
    public class Tree : TileType
    {
        public override string Name { get { return "Tree"; } }

        public override bool CanMoveOn { get { return false; } }

        public override bool CanShootOver { get { return false; } }

        public override char StringRepresentation
        {
            get { return 'T'; }
        }
    }
}