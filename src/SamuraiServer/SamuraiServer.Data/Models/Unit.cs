using System;

namespace SamuraiServer.Data
{
    public abstract class Unit
    {
        protected Unit()
        {
            this.CurrentHitPoints = this.HitPoints;
        }

        public Guid Id { get; set; }
        public abstract string Name { get; }
               
        public abstract string ImageSpriteResource { get; }

        public int Moves { get; set; }
        public int Size { get; set; }
        public double Attack { get; set; }
        public double Defence { get; set; }
        public double Range { get; set; }

        public double HitPoints { get; set; }
               
        public double CurrentHitPoints { get; set; }
               
        public int X { get; set; }
        public int Y { get; set; }
    }
}
