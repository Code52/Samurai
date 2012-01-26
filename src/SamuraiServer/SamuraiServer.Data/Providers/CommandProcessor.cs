using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiServer.Data.Providers
{
    public class CommandProcessor
    {
        private readonly GameState _match;

        public CommandProcessor(GameState match)
        {
            _match = match;
        }

        public void Process(IEnumerable<dynamic> commands)
        {
            foreach (var c in commands)
            {
                if (c.type == "move")
                {
                    ProcessMove(c);
                }
                //if (c.type == "attack")
                //{
                //    ProcessAttack(c);
                //}
                //if (c.type == "create")
                //{
                //    ProcessCreate(c);
                //}


            }


        }

        private void ProcessMove(dynamic o)
        {
            string unitId = o.unitId.ToString(); // mismatch between this and "unit-id" from JSON
            Guid id;
            if (!Guid.TryParse(unitId, out id))
            {

            }
        }

        public Unit MoveUnit(Guid id, int x, int y)
        {
            var foundUnit = _match.Players.SelectMany(c => c.Units).FirstOrDefault(g => g.Id == id);

            if (foundUnit != null)
            {
                if (!IsCurrentPlayer(foundUnit))
                    return null;

                var newCoordinates = new Point(x, y);
                var currentCoordinates = new Point(foundUnit.X, foundUnit.Y);
                var distance = newCoordinates.DistanceFrom(currentCoordinates);

                if (distance > foundUnit.Range)
                    return null;

                foundUnit.X = x;
                foundUnit.Y = y;
            }

            return foundUnit;
        }

        public void Attack(Guid id, Guid targetId)
        {
            // TODO: get units associated with ids
            // TODO: check target is within range of attacker
            // TODO: execute damage on target
            // TODO: 
        }

        private bool IsCurrentPlayer(Unit foundUnit)
        {
            var playerOwningUnit = _match.Players.FirstOrDefault(c => c.Units.Contains(foundUnit));
            if (playerOwningUnit == null)
                return false;

            return _match.Players[_match.Turn] == playerOwningUnit;
        }
    }
}

