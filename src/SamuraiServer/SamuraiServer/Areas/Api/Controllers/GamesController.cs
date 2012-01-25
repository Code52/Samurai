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
        private readonly GameStateProvider _prov;

        public GamesController(GameStateProvider prov)
        {
            _prov = prov;
        }

        [Api]
        [HttpPost]
        public ActionResult CreateGame(string name)
        {
            var state = new GameState { Name = name };
            _prov.Save(state);
            return View(new { ok = true });
        }

        [Api]
        [HttpPost]
        public ActionResult JoinGame(Guid gameId, string userName)
        {
            return View(new { ok = true });
        }

        [Api]
        [HttpPost]
        public ActionResult LeaveGame(Guid gameId, string userName)
        {
            var currentGame = _prov.ListCurrentGames(userName).FirstOrDefault(g => g.Id == gameId);

            if (currentGame == null)
                return View(new { ok = false, message = "Game does not exist" });

            var player = currentGame.Players.FirstOrDefault(f => f.Player.Name == userName);

            if (player == null)
                return View(new {ok = false, message = "Player is not in this game"});
                
            currentGame.Players.Remove(player);

            _prov.Save(currentGame);
            

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
                currentGames = _prov.ListCurrentGames(userName);
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
            return View(new { games = _prov.ListOpenGames() });
        }
    }
}
