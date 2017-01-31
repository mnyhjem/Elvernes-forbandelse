using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Creatures;
using ElvenCurse.Core.Model.Creatures.Npcs;
using ElvenCurse.Core.Model.Items;
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
            var model = new Character(Creaturetype.Hunter);

            model.CharacterAppearance = new CharacterAppearance
            {
                Sex = Sex.Female,
                Hair = new Hair {Color = HairColor.Blonde, Type=Hair.HairType.Long},
                Body = Body.Light,
                Ears = Ears.Elvenears,
                Eyecolor = Eyecolor.Blue,
                Nose = Nose.Default,
            };
            
            Session["dressingroomCharacter"] = model;

            return View("Edit", model);
        }

        public ActionResult UpdateModelAppearence(Character model)
        {
            Session["dressingroomCharacter"] = model;

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult Create(Character model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("Edit");
            //}

            var character = Session["dressingroomCharacter"] as Character;
            if (character == null)
            {
                // fail
                return View("Edit");
            }

            character.Name = model.Name;

            if (!_characterService.CreateNewCharacter(User.Identity.GetUserId(), character))
            {
                // fail
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