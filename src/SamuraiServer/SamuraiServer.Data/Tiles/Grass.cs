﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Tiles
{
    public class Grass : TileType
    {
        public override string Name { get { return "Grass"; } }

        public override bool CanMoveOn { get { return true; } }

        public override bool CanShootOver { get { return true; } }

        public override char StringRepresentation
        {
            get { return '.'; }
        }
    }
}