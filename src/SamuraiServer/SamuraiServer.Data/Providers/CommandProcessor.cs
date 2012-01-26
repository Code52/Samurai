using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiServer.Data.Providers
{
    public class CommandResult
    {
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<ValidationResult> Errors { get; set; }
    }

    public class CommandProcessor
    {
        private readonly GameState _match;

        public CommandProcessor(GameState match)
        {
            _match = match;
        }

        public CommandResult Process(IEnumerable<dynamic> commands)
        {
            var units = new List<Unit>();
            var errors = new List<ValidationResult>();

            foreach (var c in commands)
            {
                if (c.action == "move")
                {
                    var result = ProcessMove(c);
                    if (result != null)
                        units.Add(result);
                    else
                    {
                        errors.Add(new ValidationResult { IsValid = false, Message = "Could not move unit" });
                    }
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


            return new CommandResult { Units = units, Errors = errors };
        }

        private Unit ProcessMove(dynamic o)
        {
            string unitId = o.unitId.ToString();
            Guid id;
            if (!Guid.TryParse(unitId, out id))
                return null;

            int x = o.X;
            int y = o.Y;

            return MoveUnit(id, x, y);
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

