using System;
using System.Web.Mvc;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class MatchController : Controller
    {
        //
        // GET: /Api/Match/

        /// <summary>
        /// get updates to the game since a specific event
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public ActionResult GetMovesSince(Guid matchId)
        {
            return View(); // list of moves and their associated players
        }

        /// <summary>
        /// Get moves which the current user can perform for a specific game
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult GetAvailableMoves(Guid matchId, string userName)
        {
            return View(); // get list of available moves for the specific user
        }

        /// <summary>
        /// Apply a move to a current game
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult SendCommand(int user) // TODO: model to map values to
        {
            return View(); // return details around the success/failure of the move
        }
    }
}
