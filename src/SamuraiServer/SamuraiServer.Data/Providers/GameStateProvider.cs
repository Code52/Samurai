using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class GameStateProvider
    {
        public readonly IGameStateRepository _repo;

        public GameStateProvider(IGameStateRepository repository)
        {
            _repo = repository;
        }

        public GameState Load(Guid id)
        {
            return _repo.Get(id);
        }

        public void Save(GameState state)
        {
            var gs = _repo.FindBy(s => s.Id == state.Id);

            if (gs == null)
            {
                state.Id = Guid.NewGuid();

                _repo.Add(state);
            }
            else
            {
                _repo.Add(state);
            }
        }

        public IEnumerable<GameState> ListOpenGames()
        {
            return _repo.GetAll();
        }

        public IEnumerable<GameState> ListCurrentGames(string userName)
        {
            return _repo.FindBy(d => d.Players.Any(c => c.Player.Name == userName));
        }
    }
}
