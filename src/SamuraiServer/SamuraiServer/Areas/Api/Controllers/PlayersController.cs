using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SamuraiServer.Data;
using MvcApi;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class PlayersController : Controller
    {
        //
        // GET: /Api/Players/

        private readonly IPlayerRepository _db;

        public PlayersController(IPlayerRepository db)
        {
            _db = db;
        }

        [Api]
        public ActionResult Leaderboard()
        {
            var leaders = _db.GetLeaderboard(0, 20);

            return View(leaders);
        }

        [Api]
        [HttpPost]
        public ActionResult CreatePlayer(string name)
        {
            var player = new Player { Name = name };
            var resultPlayer = _db.Create(player);

            return View(new { ok = true, player = resultPlayer });
        }
    }
}
