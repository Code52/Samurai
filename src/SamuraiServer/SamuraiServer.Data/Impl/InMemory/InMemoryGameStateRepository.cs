using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryGameStateRepository : IGameStateRepository
    {
        private static readonly Dictionary<Guid, GameState> State = new Dictionary<Guid, GameState>();

        public InMemoryGameStateRepository()
        {
            State.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Players = new List<GamePlayer>(), Turn = 0 });
            State.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Players = new List<GamePlayer>(), Turn = 0 });
            State.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Players = new List<GamePlayer>(), Turn = 0 });
            State.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Players = new List<GamePlayer>(), Turn = 0 });
            State.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Players = new List<GamePlayer>(), Turn = 0 });
            State.Add(Guid.NewGuid(), new GameState { Id = Guid.NewGuid(), Players = new List<GamePlayer>(), Turn = 0 });
        }

        public IQueryable<GameState> GetAll()
        {
            return State.Values.AsQueryable();
        }

        public IQueryable<GameState> FindBy(Expression<Func<GameState, bool>> predicate)
        {
            return State.Values.AsQueryable().Where(predicate);
        }

        public GameState Get(Guid id)
        {
            return State[id];
        }

        public GameState GetByName(string name)
        {
            return State.Values.FirstOrDefault(gs => gs.Name == name);
        }

        public void Add(GameState entity)
        {
            State.Add(entity.Id, entity);
        }

        public void Delete(Guid id)
        {
            State.Remove(id);
        }

        public void Edit(GameState entity)
        {
            var gameState = State[entity.Id];

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
