using System.Web.Mvc;
using SamuraiServer.Data;
using MvcApi;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class PlayersController : Controller
    {
        //
        // GET: /Api/Players/

        private readonly PlayersProvider _prov;

        public PlayersController(PlayersProvider prov)
        {
            _prov = prov;
        }

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
