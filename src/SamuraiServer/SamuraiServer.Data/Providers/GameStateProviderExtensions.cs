using System;

namespace SamuraiServer.Data
{
    public static class GameStateProviderExtensions
    {
        public static GameState CreateAndJoin(this IGameStateProvider gameStateProvider, string name, Guid playerId)
        {
            var game = gameStateProvider.CreateGame(name);
            return gameStateProvider.JoinGame(game.Id, playerId);
        }
    }
}