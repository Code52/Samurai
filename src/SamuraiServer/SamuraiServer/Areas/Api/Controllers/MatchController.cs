using System;
using System.Web.Mvc;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class MatchController : Controller
    {
        /// <summary>
        /// Indicate that the players associated with a game are ready to start
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Start(Guid matchId)
        {
            // check if a game exists
            // check if it is not started
            // check if there is more than 1 participant available

            return Json(new { status = false });
        }


        /// <summary>
        /// get updates to the game since a specific event
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public ActionResult GetMovesSince(Guid matchId)
        {
            return Json(new { ok = false }, JsonRequestBehavior.AllowGet); // list of moves and their associated players
        }

        /// <summary>
        /// Get moves which the current user can perform for a specific game
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult GetAvailableMoves(Guid matchId, string userName)
        {
            return Json(new { ok = false }, JsonRequestBehavior.AllowGet); // get list of available moves for the specific user
        }

        /// <summary>
        /// Apply a move to a current game
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult SendCommand(int user) // TODO: model to map values to
        {
            return Json(new { ok = false }, JsonRequestBehavior.AllowGet); // return details around the success/failure of the move
        }
    }
}
