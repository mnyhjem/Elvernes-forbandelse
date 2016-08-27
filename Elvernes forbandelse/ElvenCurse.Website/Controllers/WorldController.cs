using System.Configuration;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using Microsoft.AspNet.Identity;

namespace ElvenCurse.Website.Controllers
{
    [Authorize]
    public class WorldController : Controller
    {
        private readonly ICharacterService _characterService;

        public WorldController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var character = _characterService.GetOnlineCharacter(User.Identity.GetUserId());
            if (character == null)
            {
                return RedirectToAction("Index", "Character");
            }

            var model = new WorldViewmodel
            {
                Serverpath = ConfigurationManager.AppSettings["serverPath"]
            };
            return View(model);
        }
    }

    public class WorldViewmodel
    {
        public string Serverpath { get; set; }
    }
}