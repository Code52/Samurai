using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcApi;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class MatchController : Controller
    {
        private readonly IGameStateRepository repo;
        //
        // GET: /Api/Match/

        public MatchController(IGameStateRepository repo)
        {
            this.repo = repo;
        }

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
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="player"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        [Api]
        [HttpPost]
        public ActionResult SendCommand(Guid gameId, string player, IEnumerable<dynamic> commands) 
        {
            var game = repo.Get(gameId);
            if (game == null)
                return View(new { status = false, error = "Game could not be found" });

            var processor = new CommandProcessor(game);

            var result = processor.Process(commands);

            // TODO: save game
            // TODO: need an endpoint for others to query 
            // TODO: error message format isn't right - needs command and message

            return View(new { status = true, data = new { gameId, player, units = result.Units, errors = result.Errors  } }); // return details around the success/failure of the move
        }
    }
}
