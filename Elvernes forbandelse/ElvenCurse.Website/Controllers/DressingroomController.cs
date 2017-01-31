using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Creatures;
using ElvenCurse.Core.Model.Creatures.Npcs;
using ElvenCurse.Core.Model.Items;

namespace ElvenCurse.Website.Controllers
{
    public class DressingroomController : Controller
    {
        private readonly IItemsService _itemsService;

        public DressingroomController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }
        
        public ActionResult Index()
        {
            var model = new DressingroomViewmodel
            {
                Character = new Character(Creaturetype.Hunter)
            };

            model.Character.CharacterAppearance = new CharacterAppearance
            {
                Sex = Sex.Female,
                Body = Body.Darkelf2,
                Ears = Ears.Elvenears,
                Eyecolor = Eyecolor.Blue,
                Nose = Nose.Default,
            };

            model.Character.Equipment = new CharacterEquipment
            {
                Chest = new Item { Imagepath = "torso/dress_female/tightdress_black" }
            };

            Session["dressingroomCharacter"] = model.Character;

            return View(model);
        }

        //[HttpPost]
        public ActionResult Update(DressingroomViewmodel model)
        {
            Session["dressingroomCharacter"] = model.Character;

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }

    public class DressingroomViewmodel
    {
        public Character Character { get; set; }
    }
}