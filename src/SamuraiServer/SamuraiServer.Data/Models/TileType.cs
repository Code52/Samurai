using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public abstract class TileType
    {
        public abstract string Name { get; }

        public abstract bool CanMoveOn { get; }

        public abstract bool CanShootOver { get; }
    }
}