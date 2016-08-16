using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElvenCurse.Website.Controllers
{
    [Authorize]
    public class WorldController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Character");
        }

        [HttpPost]
        public ActionResult Index(int selectedCharacterId)
        {
            return View(selectedCharacterId);
        }
    }
}