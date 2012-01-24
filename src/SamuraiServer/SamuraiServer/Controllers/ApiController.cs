using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SamuraiServer.Data;

namespace SamuraiServer.Controllers
{
    public class ApiController : Controller
    {
        private readonly IGameStateRepository db;

        public ApiController(IGameStateRepository db) {
            this.db = db;
        }

        [HttpPost]
        public ActionResult CreateGame(string name) {
            var state = new GameState { Name = name };
            db.Save(state);
            return Json(new { ok = true });
        }

        public ActionResult ListGames() {
            return Json(db.ListCurrentGames());
        }
    }
}
