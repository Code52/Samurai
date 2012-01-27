using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class TileType
    {
        public string Name { get; set; }

        public bool CanMoveOn { get; set; }
		
        public abstract bool CanShootOver { get; }

        public abstract char StringRepresentation{ get; }
    }
}