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
        private readonly IGameStateRepository _db;

        public GamesController(IGameStateRepository db)
        {
            _db = db;
        }

        [Api]
        [HttpPost]
        public ActionResult CreateGame(string name)
        {
            var state = new GameState { Name = name };
            _db.Save(state);
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
            var possibleGames = _db.ListCurrentGames(userName).Where(g => g.Id == gameId);

            if (!possibleGames.Any())
                return View(new { ok = false });

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
                currentGames = _db.ListCurrentGames(userName);
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
            return View(new { games = _db.ListOpenGames() });
        }
    }
}
