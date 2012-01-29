using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class Samurai: Unit
    {
        public Samurai()
        {
            Id = Guid.NewGuid();
            Attack = 5;
            Defence = 4;
            HitPoints = 30;
            CurrentHitPoints = 30;
            Moves = 2;
        }

        public override Guid Id { get; set; }

        public override string Name { get { return "Samurai"; } }

        public override string ImageSpriteResource
        {
            get { throw new NotImplementedException(); }
        }

        public override int Size
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override int Moves { get; set; }

        public override double Attack { get; set; }

        public override double Defence { get; set; }

        public override double Range { get; set; }

        public override double HitPoints { get; set; }

        public override double CurrentHitPoints { get; set; }

        public override int X { get; set; }

        public override int Y { get; set; }
    }
}
