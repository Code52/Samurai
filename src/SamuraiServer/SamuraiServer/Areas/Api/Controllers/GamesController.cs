using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SamuraiServer.Data;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameStateProvider _gameStateProvider;

        public GamesController(IGameStateProvider prov)
        {
            _gameStateProvider = prov;
        }

        [HttpPost]
        public ActionResult CreateGameAndJoin(string name, Guid playerId)
        {
            try
            {
                var result = _gameStateProvider.CreateGame(name);
                if (result.IsValid == false)
                    return Json(new { ok = false, message = result.Message });

                var gameState = _gameStateProvider.JoinGame(result.Data.Id, playerId);
                if (gameState.IsValid == false)
                    return Json(new { ok = false, message = gameState.Message });

                return Json(new { ok = true, game = gameState.Data });
            }
            catch
            {
                return Json(new { ok = false });
            }
        }

        [HttpPost]
        public ActionResult CreateGame(string name)
        {
            try
            {
                var gameState = _gameStateProvider.CreateGame(name);
                return Json(new { ok = true, game = gameState.Data });
            }
            catch
            {
                return Json(new { ok = false });
            }
        }

        [HttpPost]
        public ActionResult JoinGame(Guid gameId, Guid playerId)
        {
            try
            {
                var result = _gameStateProvider.JoinGame(gameId, playerId);
                if (result.IsValid == false)
                    return Json(new { ok = false, message = result.Message });

                return Json(new { ok = true, game = result.Data });
            }
            catch
            {
                return Json(new { ok = false });
            }
        }

        [HttpPost]
        public ActionResult LeaveGame(Guid gameId, string userName)
        {
            var result = _gameStateProvider.LeaveGame(gameId, userName);
            if (result.IsValid == false)
                return Json(new { ok = false, message = result.Message });

            return Json(new { ok = true });
        }

        [HttpPost]
        public ActionResult GetGames(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return Json(new { ok = false });

            IEnumerable<GameState> currentGames;

            try
            {
                currentGames = _gameStateProvider.ListCurrentGames(userName);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, message = ex.Message });

            }
            return Json(new { ok = true, games = currentGames });
        }

        public ActionResult GetOpenGames()
        {
            return Json(new { ok = true, games = _gameStateProvider.ListOpenGames() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetMap(Guid mapId)
        {
            var result = _gameStateProvider.GetMap(mapId);
            if(!result.IsValid ?? false)
            {
                return Json(new {ok = false});
            }

            return Json(new {ok = true, map = result.Data});
        }

        public ActionResult StartGame(Guid gameId)
        {
            var result = _gameStateProvider.StartGame(gameId);

            if (result.IsValid == false) 
                return Json(new { ok = false, message = result.Message }, JsonRequestBehavior.AllowGet);

            return Json(new { ok = true, game = result.Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGame(Guid gameId)
        {
            var result = _gameStateProvider.Load(gameId);
            if (result == null)
            {
                return Json(new { ok = false, message = "Game not found" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ok = true, game = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
