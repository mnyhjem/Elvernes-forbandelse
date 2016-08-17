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
            return View();
        }
    }
}