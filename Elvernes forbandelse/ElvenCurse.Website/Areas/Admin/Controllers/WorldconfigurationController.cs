using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Tilemap;
using ElvenCurse.Website.Areas.Admin.Models;

namespace ElvenCurse.Website.Areas.Admin.Controllers
{
    public class WorldconfigurationController : Controller
    {
        private readonly IWorldService _worldService;

        public WorldconfigurationController(IWorldService worldService)
        {
            _worldService = worldService;
        }

        // GET: Admin/World
        public ActionResult Index()
        {
            var maps = _worldService.GetMaps();
            var terrains = _worldService.GetTerrains();

            var model = new WorldconfigurationViewmodel
            {
                Worldsections = maps,
                Terrains = terrains
            };
            //var test = Newtonsoft.Json.JsonConvert.DeserializeObject(map.Json);
            //var test2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Tilemap>(map.Json);

            return View(model);
        }

        public ActionResult Create()
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var map = _worldService.GetMap(id);
            if (map == null)
            {
                return HttpNotFound("Map not found");
            }

            var allmaps = _worldService.GetMaps();
            allmaps.Insert(0, new Worldsection {Id = 0, Name = "Ingen"});

            var model = new EditWorldsectionViewmodel
            {
                Worldsection = map,
                Worldsections = new SelectList(allmaps, "Id", "Name")
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditWorldsectionViewmodel model)
        {
            if (model.Worldsection.Id > 0 && _worldService.GetMap(model.Worldsection.Id) == null)
            {
                return HttpNotFound("Map not found");
            }

            var mapdata = "";
            if (Request.Files.Count > 0)
            {
                mapdata = System.Text.Encoding.Default.GetString(ReadFully(Request.Files[0].InputStream));
            }

            if (!_worldService.SaveMap(model.Worldsection, mapdata))
            {
                var allmaps = _worldService.GetMaps();
                allmaps.Insert(0, new Worldsection { Id = 0, Name = "Ingen" });
                model.Worldsections = new SelectList(allmaps, "Id", "Name");
                ModelState.AddModelError("", "Kunne ikke gemme");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        private static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        [HttpGet]
        public ActionResult Createterrain()
        {
            var model = new Terrainfile();
            return View("Editterrain", model);
        }

        [HttpGet]
        public ActionResult Editterrain(int id)
        {
            var terrain = _worldService.GetTerrain(id);
            if (terrain == null)
            {
                return HttpNotFound("Terrain not found");
            }
            return View(terrain);
        }

        [HttpPost]
        public ActionResult Editterrain(Terrainfile model)
        {
            if (model.Id > 0 && _worldService.GetTerrain(model.Id) == null)
            {
                return HttpNotFound("Terrain not found");
            }

            var data = "";
            if (Request.Files.Count > 0)
            {
                data = System.Text.Encoding.Default.GetString(ReadFully(Request.Files[0].InputStream));
                model.Filename = Request.Files[0].FileName;
            }

            if (!_worldService.SaveTerrain(model, data))
            {
                ModelState.AddModelError("", "Kunne ikke gemme");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }

    public class EditWorldsectionViewmodel
    {
        public Worldsection Worldsection { get; set; }
        public SelectList Worldsections { get; set; }
    }
}