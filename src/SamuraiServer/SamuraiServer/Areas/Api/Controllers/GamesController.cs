using System.Web.Mvc;
using SamuraiServer.Data;
using MvcApi;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameStateRepository _db;

        public GamesController(IGameStateRepository db) {
            _db = db;
        }

        [Api]
        [HttpPost]
        public ActionResult CreateGame(string name) {
            var state = new GameState { Name = name };
            _db.Save(state);
            return View(new { ok = true });
        }

        [Api]
        public ActionResult ListGames() {
            return View(_db.ListCurrentGames());
        }
    }
}
