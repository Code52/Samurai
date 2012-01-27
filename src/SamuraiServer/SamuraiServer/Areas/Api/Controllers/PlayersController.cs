using System.Web.Mvc;
using SamuraiServer.Data;
using MvcApi;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class PlayersController : Controller
    {
        //
        // GET: /Api/Players/

        private readonly IPlayersProvider _prov;

        public PlayersController(IPlayersProvider prov)
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
            var result = _prov.Create(name);
            if (result.IsValid == false) return View(new { ok = false, message = result.Message });

            return View(new { ok = true, player = result.Data });
        }

        [Api]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Login(string name, string token)
        {
            var result = _prov.Login(name, token);

            if (result.IsValid == false)
            {
                // Login didn't go through
                return View(new { ok = false, message = result.Message });
            }
            else
            {
                // Login ok, send the player-object back
                return View(new { ok = true, message = result.Message });
            }
        }
    }
}
