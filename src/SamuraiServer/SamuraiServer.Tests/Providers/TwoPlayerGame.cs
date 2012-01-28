using SamuraiServer.Data;
using SamuraiServer.Data.Providers;

namespace SamuraiServer.Tests.Providers
{
    public abstract class TwoPlayerGame : SpecificationFor<CommandProcessor>
    {
        public override void SetUp()
        {
            FirstPlayer = new GamePlayer();
            SecondPlayer = new GamePlayer();
            State = new GameState();

            State.Players.Add(FirstPlayer);
            State.Players.Add(SecondPlayer);
        }

        protected GameState State { get; private set; }

        public GamePlayer FirstPlayer { get; private set; }
        public GamePlayer SecondPlayer { get; private set; }
    }
}