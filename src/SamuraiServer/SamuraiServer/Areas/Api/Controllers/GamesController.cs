using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SamuraiServer.Data;
using MvcApi;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameStateProvider _gameStateProvider;
        private readonly IPlayersProvider _playersProvider;

        public GamesController(IGameStateProvider prov, IPlayersProvider playersProvider)
        {
            _gameStateProvider = prov;
            _playersProvider = playersProvider;
        }

        [Api]
        [HttpPost]
        public ActionResult CreateGameAndJoin(string name, Guid playerId)
        {
            try
            {
                var gameState = _gameStateProvider.CreateGame(name);
                gameState = _gameStateProvider.JoinGame(gameState.Id, playerId);
                return View(new {ok = true, game = gameState});
            }
            catch
            {
                return View(new {ok = false});
            }
        }

        [Api]
        [HttpPost]
        public ActionResult CreateGame(string name)
        {
            try
            {
                var gameState = _gameStateProvider.CreateGame(name);
                return View(new { ok = true, game = gameState });
            }
            catch
            {
                return View(new { ok = false });
            }
        }

        [Api]
        [HttpPost]
        public ActionResult JoinGame(Guid gameId, Guid playerId)
        {
            try
            {
                var game = _gameStateProvider.JoinGame(gameId, playerId);
                return View(new {ok = true, game});
            }
            catch
            {
                return View(new {ok = false});
            }
        }

        [Api]
        [HttpPost]
        public ActionResult LeaveGame(Guid gameId, string userName)
        {
            var currentGame = _gameStateProvider.ListCurrentGames(userName).FirstOrDefault(g => g.Id == gameId);

            if (currentGame == null)
                return View(new { ok = false, message = "Game does not exist" });

            var player = currentGame.Players.FirstOrDefault(f => f.Player.Name == userName);

            if (player == null)
                return View(new {ok = false, message = "Player is not in this game"});
                
            currentGame.Players.Remove(player);

            _gameStateProvider.Save(currentGame);
            

            return View(new { ok = true });
        }

        [Api]
        [HttpPost]
        public ActionResult GetGames(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return View(new { ok = false });

            IEnumerable<GameState> currentGames;

            try
            {
                currentGames = _gameStateProvider.ListCurrentGames(userName);
            }
            catch (Exception)
            {
                return View(new {ok = false});

            }
            return View(new { ok = true, games = currentGames });
        }

        [Api]
        public ActionResult GetOpenGames()
        {
            return View(new { games = _gameStateProvider.ListOpenGames() });
        }
    }
}
