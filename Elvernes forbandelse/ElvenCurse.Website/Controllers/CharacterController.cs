using System.Web;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ElvenCurse.Website.Controllers
{
    [Authorize]
    public class CharacterController : Controller
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        // GET: Character
        public ActionResult Index()
        {
            var characters = _characterService.GetCharactersForUser(User.Identity.GetUserId());
            return View(characters);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(ElvenCurse.Core.Model.Character model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }

            if (!_characterService.CreateNewCharacter(User.Identity.GetUserId(), model))
            {
                return View("Edit");
            }

            return RedirectToAction("Index");
        }
    }
}