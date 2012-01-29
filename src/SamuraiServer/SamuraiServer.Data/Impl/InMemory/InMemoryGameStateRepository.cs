using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryGameStateRepository : IGameStateRepository
    {
        private static readonly Dictionary<Guid, GameState> State = new Dictionary<Guid, GameState>();

        public InMemoryGameStateRepository(IMapProvider maps)
        {
            if (State.Count == 0)
            {
                Guid id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Turn = 0, Name = "Game 1", MapId = maps.GetRandomMap().Id });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Turn = 0, Name = "Game 2", MapId = maps.GetRandomMap().Id });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Turn = 0, Name = "Game 3", MapId = maps.GetRandomMap().Id });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Turn = 0, Name = "Game 4", MapId = maps.GetRandomMap().Id });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Turn = 0, Name = "Game 5", MapId = maps.GetRandomMap().Id });
                id = Guid.NewGuid();
                State.Add(id, new GameState { Id = id, Turn = 0, Name = "Game 6", MapId = maps.GetRandomMap().Id });
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
