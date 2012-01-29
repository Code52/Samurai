﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiServer.Data.Providers
{
    public class AttackResult
    {
        public Unit Source { get; set; }
        public Unit Target { get; set; }
    }

    public class ErrorMessage
    {
        public Guid UnitId { get; set; }
        public string Message { get; set; }
    }

    public class CommandResult
    {
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<ErrorMessage> Errors { get; set; }
        public IEnumerable<string> Notifications { get; set; }
    }

    public class CommandProcessor
    {
        private readonly ICombatCalculator calculator;
        private readonly GameState _match;

        public CommandProcessor(ICombatCalculator calculator, GameState match)
        {
            this.calculator = calculator;
            _match = match;
        }

        public CommandResult Process(IEnumerable<dynamic> commands)
        {
            var units = new List<Unit>();
            var errors = new List<ErrorMessage>();
            IEnumerable<string> notifications = new List<string>();

            if (commands == null)
                return new CommandResult { Units = units, Errors = errors, Notifications = notifications };

            foreach (var c in commands)
            {
                string unitId = c.unitId.ToString();
                Guid id;
                if (!Guid.TryParse(unitId, out id))
                    continue; // cannot parse command

                if (c.action == "move")
                {
                    ValidationResult<Unit> result = ProcessMove(c);
                    if (result.IsValid == true)
                        units.Add(result.Data);
                    else
                    {
                        errors.Add(new ErrorMessage { UnitId = id, Message = result.Message });
                    }
                }

                if (c.action == "attack")
                {
                    ValidationResult<AttackResult> result = ProcessAttack(c);
                    if (result.IsValid == true)
                    {
                        units.Add(result.Data.Source);
                        units.Add(result.Data.Target);
                    }
                    else
                    {
                        errors.Add(new ErrorMessage { UnitId = id, Message = result.Message });
                    }
                }
                //if (c.type == "create")
                //{
                //    ProcessCreate(c);
                //}
            }

            notifications = GetPostRoundEvents();

            return new CommandResult { Units = units, Errors = errors, Notifications = notifications };
        }

        private IEnumerable<string> GetPostRoundEvents()
        {
            var events = new List<string>();

            var playersWithoutUnits = _match.Players.Where(c => c.Units.All(u => u.CurrentHitPoints == 0.0)).ToList();

            foreach (var player in playersWithoutUnits)
            {
                if (player.Player != null)
                {
                    events.Add(string.Format("{0} has been eliminated", player.Player.Name));    
                }



                _match.Players.Remove(player);
            }

            return events;
        }

        private ValidationResult<AttackResult> ProcessAttack(dynamic command)
        {
            string unitId = command.unitId.ToString();
            Guid id;
            if (!Guid.TryParse(unitId, out id))
                return null;

            int x = command.X;
            int y = command.Y;

            return AttackUnit(id, x, y);
        }

        private ValidationResult<Unit> ProcessMove(dynamic o)
        {
            string unitId = o.unitId.ToString();
            Guid id;
            if (!Guid.TryParse(unitId, out id))
                return null;

            int x = o.X;
            int y = o.Y;

            return MoveUnit(id, x, y);
        }

        private ValidationResult<Unit> MoveUnit(Guid id, int x, int y)
        {
            var foundUnit = _match.Players.SelectMany(c => c.Units).FirstOrDefault(g => g.Id == id);

            if (foundUnit == null)
            {
                return ValidationResult<Unit>.Failure("Could not find unit '{0}'", id);
            }

            if (!IsCurrentPlayer(foundUnit))
            {
                return ValidationResult<Unit>.Failure("This player does not have the right to move");
            }

            var newCoordinates = new Point(x, y);
            var currentCoordinates = new Point(foundUnit.X, foundUnit.Y);
            var distance = newCoordinates.DistanceFrom(currentCoordinates);

            if (distance > foundUnit.Range)
            {
                return ValidationResult<Unit>.Failure("Unit cannot move that distance").WithData(foundUnit);
            }

            foundUnit.X = x;
            foundUnit.Y = y;

            return ValidationResult<Unit>.Success.WithData(foundUnit);
        }

        private ValidationResult<AttackResult> AttackUnit(Guid id, int x, int y)
        {
            var allUnits = _match.Players.SelectMany(p => p.Units).ToList();

            var attackUnit = allUnits.FirstOrDefault(u => u.Id == id);
            var targetUnit = allUnits.FirstOrDefault(u => u.X == x && u.Y == y);

            if (attackUnit == null)
                return ValidationResult<AttackResult>.Failure("Could not find attack unit");

            if (targetUnit == null)
            {
                return ValidationResult<AttackResult>.Failure(string.Format("No unit found at this location [{0},{1}]", x, y));
            }

            // TODO: check target is within range of attacker

            targetUnit.CurrentHitPoints -= calculator.CalculateDamage(attackUnit, targetUnit);
            if (targetUnit.CurrentHitPoints < 0) targetUnit.CurrentHitPoints = 0;

            var result = new AttackResult { Source = attackUnit, Target = targetUnit };

            return ValidationResult<AttackResult>.Success.WithData(result);
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

