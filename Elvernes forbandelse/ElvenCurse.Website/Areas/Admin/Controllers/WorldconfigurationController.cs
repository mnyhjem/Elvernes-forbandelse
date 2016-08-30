using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Tilemap;

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

            //var test = Newtonsoft.Json.JsonConvert.DeserializeObject(map.Json);
            //var test2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Tilemap>(map.Json);

            return View(maps);
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

            if (Request.Files.Count > 0)
            {
                var json = System.Text.Encoding.Default.GetString(ReadFully(Request.Files[0].InputStream));

                // lille fiks for at få vores tsx fil til at virke..
                json = json.Replace(@"""source"":""Terrain.tsx""", @"""columns"":32,
         ""image"":""terrain.png"",
         ""imageheight"":1024,
         ""imagewidth"":1024,
         ""margin"":0,
         ""name"":""terrain"",
         ""spacing"":0,
         ""tilecount"":1024,
         ""tileheight"":32,
         ""tilewidth"":32");

                model.Worldsection.Json = json;
            }

            if (!_worldService.SaveMap(model.Worldsection))
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
    }

    public class EditWorldsectionViewmodel
    {
        public Worldsection Worldsection { get; set; }
        public SelectList Worldsections { get; set; }
    }
}