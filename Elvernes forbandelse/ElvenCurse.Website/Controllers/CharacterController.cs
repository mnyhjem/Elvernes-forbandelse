using System.Collections.Generic;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using Microsoft.AspNet.Identity;

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
        public ActionResult Index(int? id)
        {
            var model = new CharacterlistViewmodel
            {
                Characters = _characterService.GetCharactersForUser(User.Identity.GetUserId()),
                SelectedCharacterId = id ?? 0
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Create(Character model)
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

        [HttpPost]
        public ActionResult EnterWorld(CharacterlistViewmodel model)
        {
            var character = _characterService.GetCharacter(User.Identity.GetUserId(), model.SelectedCharacterId);
            if (character == null)
            {
                return RedirectToAction("Index");
            }

            _characterService.SetCharacterOnline(User.Identity.GetUserId(), model.SelectedCharacterId);

            return RedirectToAction("Index", "World");
        }
    }

    public class CharacterlistViewmodel
    {
        public List<Character> Characters { get; set; }
        public int SelectedCharacterId { get; set; }
    }
}