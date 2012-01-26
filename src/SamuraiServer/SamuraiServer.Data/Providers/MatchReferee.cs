using System;
using System.Linq;

namespace SamuraiServer.Data.Providers
{
    public class MatchReferee
    {
        private readonly GameState _match;

        public MatchReferee(GameState match)
        {
            _match = match;
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
                
                // TODO: we need to check that the move is valid
                // TODO: how do we specify the "range" of a unit's movement?
                // TODO: how do we specify the destination of a move is valid?

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

