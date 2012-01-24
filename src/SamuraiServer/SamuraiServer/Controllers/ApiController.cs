using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SamuraiServer.Data;

namespace SamuraiServer.Controllers
{
    public class ApiController : Controller
    {
        private readonly IGameStateRepository db;

        public ApiController(IGameStateRepository db) {
            this.db = db;
        }

    }
}
