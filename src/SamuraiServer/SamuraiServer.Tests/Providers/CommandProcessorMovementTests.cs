using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class CommandProcessorMovementTests
    {
        public class Ninja : Unit { }

        const string moveCommandTemplate = " [{{\"unitId\": \"{0}\",\"action\":\"move\",\"X\":1,\"Y\":0}}] ";

        public class Client_Sends_Message_To_Move_A_Unit : TwoPlayerGame
        {
            Ninja activeUnit;
            Guid id;

            public override CommandProcessor Given()
            {
                id = Guid.NewGuid();

                activeUnit = new Ninja { Id = id, X = 0, Y = 0, Range = 1 };

                FirstPlayer.Units.Add(activeUnit);

                return new CommandProcessor(State);
            }

            CommandResult result;
            Unit unit;

            public override void When()
            {
                var json = String.Format(moveCommandTemplate, id);
                var obj = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(json);
                result = Subject.Process(obj);
                unit = result.Units.First();
            }

            [Fact]
            public void The_Result_Has_One_Unit()
            {
                Assert.True(result.Units.Count() == 1);
            }

            [Fact]
            public void The_Unit_Has_New_Coordinates()
            {
                Assert.Equal(1, unit.X);
                Assert.Equal(0, unit.Y);
            }

            [Fact]
            public void The_Unit_Received_Is_The_Original_Unit()
            {
                Assert.Equal(id, unit.Id);
            }
        }

        public class Client_Attempts_To_Move_Unit_When_Not_Permitted : TwoPlayerGame
        {
            Guid id;
            Ninja activeUnit;
            Ninja otherUnit;
            CommandResult result;

            public override CommandProcessor Given()
            {
                id = Guid.NewGuid();
                activeUnit = new Ninja { Id = id, X = 0, Y = 0 };
                otherUnit = new Ninja { Id = Guid.NewGuid(), X = 1, Y = 1 };

                FirstPlayer.Units.Add(activeUnit);
                SecondPlayer.Units.Add(otherUnit);

                State.Turn = State.Players.IndexOf(SecondPlayer); // not first player's turn

                return new CommandProcessor(State);
            }

            public override void When()
            {
                var json = String.Format(moveCommandTemplate, id);
                var obj = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(json);
                result = Subject.Process(obj);
            }

            [Fact]
            public void An_Error_Message_Is_Returned()
            {
                Assert.True(result.Errors.Any());
            }
        }

        public class When_A_Unit_Moves_More_Than_Its_Allowed_Range : TwoPlayerGame
        {
            const string moveCommandTemplate = " [{{\"unitId\": \"{0}\",\"action\":\"move\",\"X\":1,\"Y\":1}}] ";

            Guid id = Guid.NewGuid();
            Ninja activeUnit;
            CommandResult result;

            public override CommandProcessor Given()
            {
                activeUnit = new Ninja { Id = id, X = 0, Y = 0 };
                activeUnit.Range = 1;

                FirstPlayer.Units.Add(activeUnit);

                return new CommandProcessor(State);
            }

            public override void When()
            {
                var json = String.Format(moveCommandTemplate, id);
                var obj = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(json);
                result = Subject.Process(obj);
            }

            [Fact]
            public void An_Error_Is_Included()
            {
                Assert.True(result.Errors.Any());
            }
        }
    }
}