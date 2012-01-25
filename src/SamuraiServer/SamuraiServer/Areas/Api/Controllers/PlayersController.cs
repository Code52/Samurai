using System.Web.Mvc;
using SamuraiServer.Data;
using MvcApi;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayersProvider _prov;

        public PlayersController(IPlayersProvider prov)
        {
            _prov = prov;
        }

        // api/players/leaderboard

        [Api]
        public ActionResult Leaderboard()
        {
            var leaders = _prov.GetLeaderboard(0, 20);

            return View(leaders);
        }

        [Api]
        [HttpPost]
        public ActionResult CreatePlayer(string name)
        {
            var player = new Player { Name = name };
            var resultPlayer = _prov.Create(player);

            return View(new { ok = true, player = resultPlayer });
        }
    }
}
