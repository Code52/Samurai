using System.Web.Mvc;
using SamuraiServer.Data;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayersProvider prov;

        public PlayersController(IPlayersProvider prov)
        {
            this.prov = prov;
        }

        public ActionResult Leaderboard()
        {
            var leaders = prov.GetLeaderboard(0, 20);

            return Json(new { ok = true, leaders }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreatePlayer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { ok = false, message = "Parameter is null or whitespace: name" });

            var result = prov.Create(name);
            if (result.IsValid == false)
                return Json(new { ok = false, message = result.Message });

            return Json(new { ok = true, player = result.Data });
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Login(string name, string token)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { ok = false, message = "Parameter is null or whitespace: name" });


            var result = prov.Login(name, token);

            if (result.IsValid == false)
            {
                // Login didn't go through
                return Json(new { ok = false, message = result.Message });
            }

            // Login ok, send the player-object back
            return Json(new { ok = true, player = result.Data });
        }
    }
}
