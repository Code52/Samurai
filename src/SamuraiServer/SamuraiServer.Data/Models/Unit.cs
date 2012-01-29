using System;

namespace SamuraiServer.Data
{
    public abstract class Unit
    {
        protected Unit()
        {
            this.CurrentHitPoints = this.HitPoints;
        }

        public abstract Guid Id { get; set; }
        public abstract string Name { get; }
               
        public abstract string ImageSpriteResource { get; }

        public abstract int Moves { get; set; }
        public abstract int Size { get; set; }
        public abstract double Attack { get; set; }
        public abstract double Defence { get; set; }
        public abstract double Range { get; set; }

        public abstract double HitPoints { get; set; }
               
        public abstract double CurrentHitPoints { get; set; }
               
        public abstract int X { get; set; }
        public abstract int Y { get; set; }
    }
}
