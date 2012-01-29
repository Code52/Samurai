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
    public class CommandProcessorNotificationTests
    {
        private const string attackCommandTemplate = "[ {{ \"unitId\": \"{0}\", \"action\":\"attack\", \"X\":10, \"Y\":5 }}]";

        public class Pirate : Unit { }

        public class When_Client_Sends_Attack_Command : TwoPlayerGame
        {
            Pirate attackUnit;
            Pirate targetUnit;
            Guid activeUnitId;
            Guid targetUnitId;

            private const double expectedDamage = 1.0;

            public override CommandProcessor Given()
            {
                var firstPlayer = new Player {Name = "shiftkey"};
                var secondPlayer = new Player { Name = "aeoth" };

                activeUnitId = Guid.NewGuid();
                targetUnitId = Guid.NewGuid();

                attackUnit = new Pirate { Id = activeUnitId, X = 10, Y = 4, Range = 1, HitPoints = 1.0, CurrentHitPoints = 1.0 };
                targetUnit = new Pirate { Id = targetUnitId, X = 10, Y = 5, Range = 1, HitPoints = 1.0, CurrentHitPoints = expectedDamage };

                FirstPlayer.Units.Add(attackUnit);
                FirstPlayer.Player = firstPlayer;
                SecondPlayer.Units.Add(targetUnit);
                SecondPlayer.Player = secondPlayer;

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
            public void An_Event_Containing_The_Second_Player_Is_Received()
            {
                Assert.Equal(1, result.Notifications.Count());
            }

            [Fact]
            public void The_Event_Is_Formatted_In_A_Certain_Way()
            {
                Assert.Equal("aeoth has been eliminated", result.Notifications.First());
            }

            [Fact]
            public void The_Second_Unit_Is_Returned_But_Has_Zero_Health()
            {
                Assert.Equal(0.0, result.Units.Last().CurrentHitPoints, 1);
            }
        }

    }
}
