using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Newtonsoft.Json;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class CommandProcessorAttackTests
    {
        public class Pirate : Unit
        {
            public Pirate()
            {
                Id = Guid.NewGuid();
                Range = 1;
                HitPoints = 1.0;
                CurrentHitPoints = 1.0;
            }

            public override string Name
            {
                get { return "Pirate"; }
            }

            public override string ImageSpriteResource
            {
                get { throw new NotImplementedException(); }
            }
        }

        private const string attackCommandTemplate = "[ {{ \"unitId\": \"{0}\", \"action\":\"attack\", \"X\":10, \"Y\":5 }}]";

        public class When_Client_Sends_Attack_Command : TwoPlayerGame
        {
            Pirate attackUnit;
            Pirate targetUnit;
            Guid activeUnitId;
            Guid targetUnitId;

            double expectedDamage = 0.9;

            public override CommandProcessor Given()
            {
                activeUnitId = Guid.NewGuid();
                targetUnitId = Guid.NewGuid();

                attackUnit = new Pirate { Id = activeUnitId, X = 10, Y = 4};
                targetUnit = new Pirate { Id = targetUnitId, X = 10, Y = 5};

                FirstPlayer.Units.Add(attackUnit);
                SecondPlayer.Units.Add(targetUnit);

                Calculator.CalculateDamage(attackUnit, targetUnit).Returns(expectedDamage);

                return new CommandProcessor(Calculator, State);
            }

            CommandResult result;

            public override void When()
            {
                var json = String.Format(attackCommandTemplate, activeUnitId);
                var obj = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(json);
                result = Subject.Process(obj);
            }

            [Fact]
            public void Both_Units_Are_Sent_To_The_Client()
            {
                Assert.Equal(2, result.Units.Count());

                Assert.Equal(activeUnitId, result.Units.First().Id);
                Assert.Equal(targetUnitId, result.Units.Last().Id);
            }

            [Fact]
            public void The_Target_Unit_Has_Taken_Damage()
            {
                Assert.Equal(0.1, targetUnit.CurrentHitPoints, 1);
            }
        }

        public class When_Client_Attempts_To_Attack_Location_Without_Target : TwoPlayerGame
        {
            Pirate activeUnit;
            Guid activeUnitId;

            public override CommandProcessor Given()
            {
                activeUnitId = Guid.NewGuid();

                activeUnit = new Pirate { Id = activeUnitId, X = 10, Y = 4 };

                FirstPlayer.Units.Add(activeUnit);

                return new CommandProcessor(Calculator, State);
            }

            CommandResult result;

            public override void When()
            {
                var json = String.Format(attackCommandTemplate, activeUnitId);
                var obj = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(json);
                result = Subject.Process(obj);
            }

            [Fact]
            public void An_Error_Is_Received_About_No_Location()
            {
                Assert.Equal(1, result.Errors.Count());
                var error = result.Errors.First();
                Assert.Equal("No unit found at this location [10,5]", error.Message);
            }
        }
    }
}