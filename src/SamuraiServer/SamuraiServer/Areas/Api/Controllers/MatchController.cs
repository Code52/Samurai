using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using SamuraiServer.Models;

namespace SamuraiServer.Areas.Api.Controllers
{
    public class MatchController : Controller
    {
        private readonly IGameStateRepository repo;
        private readonly ICombatCalculator calculator;
        
        public MatchController(IGameStateRepository repo, ICombatCalculator calculator)
        {
            this.repo = repo;
            this.calculator = calculator;
        }

        [HttpPost]
        public ActionResult SendCommands(Guid gameId, string player, [DynamicJson] IEnumerable<dynamic> commands) 
        {
            var game = repo.Get(gameId);
            if (game == null)
                return Json(new { status = false, error = "Game could not be found" });

            var processor = new CommandProcessor(calculator, game);

            var result = processor.Process(commands);

            // TODO: save game
            repo.Edit(game);
            repo.Save();
            // TODO: error message format isn't right - needs command and message

            return Json(new { status = true, data = new { gameId, player, units = result.Units, errors = result.Errors, notifications = result.Notifications } });
        }
    }
}
