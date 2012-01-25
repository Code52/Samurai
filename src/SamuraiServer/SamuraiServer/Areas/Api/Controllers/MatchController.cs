using System;
using System.Web.Mvc;
using MvcApi;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class MatchController : Controller
    {
        //
        // GET: /Api/Match/

        /// <summary>
        /// Indicate that the players associated with a game are ready to start
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        [Api]
        public ActionResult Start(Guid matchId)
        {
            // check if a game exists
            // check if it is not started
            // check if there is more than 1 participant available

            return View(new { status = true });
        }


        /// <summary>
        /// get updates to the game since a specific event
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        [Api]
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
