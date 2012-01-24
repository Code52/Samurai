using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public abstract class Unit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ImageSpriteResource { get; set; }

        public int Size { get; set; }
        public double Strength { get; set; }
        public double Armor { get; set; }
        public double Range { get; set; }

        public double HitPoints { get; set; }

        public double CurrentHitPoints { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
