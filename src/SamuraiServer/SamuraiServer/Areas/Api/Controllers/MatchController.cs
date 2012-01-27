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
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="player"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendCommands(Guid gameId, string player, IEnumerable<dynamic> commands) 
        {
            var game = repo.Get(gameId);
            if (game == null)
                return Json(new { status = false, error = "Game could not be found" });

            var processor = new CommandProcessor(game);

            var result = processor.Process(commands);

            // TODO: save game
            // TODO: need an endpoint for others to query 
            // TODO: error message format isn't right - needs command and message

            return Json(new { status = true, data = new { gameId, player, units = result.Units, errors = result.Errors } }); // return details around the success/failure of the move
        }
    }
}
