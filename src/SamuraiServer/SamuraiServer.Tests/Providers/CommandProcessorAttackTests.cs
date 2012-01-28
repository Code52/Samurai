using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class CommandProcessorAttackTests
    {
        public class Pirate : Unit { }

        private const string attackCommandTemplate = "[ {{ \"unitId\": \"{0}\", \"action\":\"attack\", \"X\":10, \"Y\":5 }}]";

        public class When_Client_Sends_Attack_Command : TwoPlayerGame
        {
            Pirate activeUnit;
            Pirate targetUnit;
            Guid activeUnitId;
            Guid targetUnitId;

            public override CommandProcessor Given()
            {
                activeUnitId = Guid.NewGuid();
                targetUnitId = Guid.NewGuid();

                activeUnit = new Pirate { Id = activeUnitId, X = 10, Y = 4, Range = 1 };
                targetUnit = new Pirate { Id = targetUnitId, X = 10, Y = 5, Range = 1 };

                FirstPlayer.Units.Add(activeUnit);
                SecondPlayer.Units.Add(targetUnit);

                return new CommandProcessor(State);
            }

            CommandResult result;

            public override void When()
            {
                var json = String.Format(attackCommandTemplate, activeUnitId);
                var obj = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(json);
                result = Subject.Process(obj);
            }

            [Fact]
            public void The_Unit_Received_Is_The_Original_Unit()
            {
                Assert.Equal(1, result.Units.Count());
            }
        }
    }
}