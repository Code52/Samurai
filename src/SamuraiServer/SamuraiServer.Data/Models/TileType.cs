using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SamuraiServer.Data.Tiles;

namespace SamuraiServer.Data
{
    public abstract class TileType
    {
        public abstract string Name { get; }

        public abstract bool CanMoveOn { get; }

        public abstract bool CanShootOver { get; }

        public static Grass G() { return new Grass(); }

        public static Rock R() { return new Rock(); }

        public static Tree T() { return new Tree(); }

        public static Water W() { return new Water(); }
    }
}