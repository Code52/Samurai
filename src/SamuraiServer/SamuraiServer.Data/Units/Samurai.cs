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

        public override string Name { get { return "Samurai"; } }

        public override string ImageSpriteResource
        {
            get { throw new NotImplementedException(); }
        }
    }
}
