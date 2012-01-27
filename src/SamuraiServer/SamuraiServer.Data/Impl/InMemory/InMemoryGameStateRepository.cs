using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryGameStateRepository : IGameStateRepository
    {
        private static Dictionary<Guid, GameState> _state = new Dictionary<Guid, GameState>();

        public InMemoryGameStateRepository()
        {
            _state.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Map = new Map(), Players = new List<GamePlayer>(), Turn = 0 });
            _state.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Map = new Map(), Players = new List<GamePlayer>(), Turn = 0 });
            _state.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Map = new Map(), Players = new List<GamePlayer>(), Turn = 0 });
            _state.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Map = new Map(), Players = new List<GamePlayer>(), Turn = 0 });
            _state.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Map = new Map(), Players = new List<GamePlayer>(), Turn = 0 });
            _state.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Map = new Map(), Players = new List<GamePlayer>(), Turn = 0 });
        }


        public IQueryable<GameState> GetAll()
        {
            return _state.Values.AsQueryable();
        }

        public IQueryable<GameState> FindBy(Expression<Func<GameState, bool>> predicate)
        {
            return _state.Values.AsQueryable().Where(predicate);
        }

        public GameState Get(Guid id)
        {
            return _state[id];
        }

        public GameState GetByName(string name)
        {
            return _state.Values.FirstOrDefault(gs => gs.Name == name);
        }

        public void Add(GameState entity)
        {
            _state.Add(entity.Id, entity);
        }

        public void Delete(Guid id)
        {
            _state.Remove(id);
        }

        public void Edit(GameState entity)
        {
            var gameState = _state[entity.Id];

            gameState.MapId = entity.MapId;
            gameState.Name = entity.Name;
            gameState.Players = entity.Players;
            gameState.Turn = entity.Turn;
        }

        public void Save()
        {
        }
    }
}
