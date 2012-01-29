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
            if (State.Count == 0)
            {
                Guid id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Players = new List<GamePlayer>(), Turn = 0, Name = "Game 1" });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Players = new List<GamePlayer>(), Turn = 0, Name = "Game 2" });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Players = new List<GamePlayer>(), Turn = 0, Name = "Game 3" });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Players = new List<GamePlayer>(), Turn = 0, Name = "Game 4" });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Players = new List<GamePlayer>(), Turn = 0, Name = "Game 5" });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Players = new List<GamePlayer>(), Turn = 0, Name = "Game 6" });
            }
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
            if(State.ContainsKey(id))
                return State[id];

            return null;
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
