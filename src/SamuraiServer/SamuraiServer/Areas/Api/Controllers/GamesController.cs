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
                var result = _gameStateProvider.CreateGame(name);
                if (result.IsValid == false) return View(new {ok = false, message = result.Message});

                var gameState = _gameStateProvider.JoinGame(result.Data.Id, playerId);
                if (gameState.IsValid == false) return View(new { ok = false, message = gameState.Message });

                return View(new {ok = true, game = gameState.Data});
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
                return View(new { ok = true, game = gameState.Data });
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
                var result = _gameStateProvider.JoinGame(gameId, playerId);
                if (result.IsValid == false) return View(new { ok = false, message = result.Message });

                return View(new {ok = true, game = result.Data});
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
            var result = _gameStateProvider.LeaveGame(gameId, userName);
            if (result.IsValid == false) return View(new { ok = false, message = result.Message });

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
            return View(new { ok = true, games = _gameStateProvider.ListOpenGames() });
        }

        [Api]
        public ActionResult StartGame(Guid gameId)
        {
            var result = _gameStateProvider.StartGame(gameId);

            if (result.IsValid == false) return View(new { ok = false, message = result.Message });

            return View(new { ok = true, game = result.Data });
        }
    }
}
