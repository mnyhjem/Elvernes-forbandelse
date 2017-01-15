using System.Collections.Generic;
using System.Web.Mvc;
using ElvenCurse.Core.Engines.Messagequeue;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;

namespace ElvenCurse.Website.Areas.Admin.Controllers
{
    public class MessagequeueController : Controller
    {
        private readonly ICharacterService _characterService;
        private readonly IWorldService _worldService;
        private readonly IMessagequeueService _messagequeueService;

        public MessagequeueController(
            ICharacterService characterService, 
            IWorldService worldService,
            IMessagequeueService messagequeueService)
        {
            _characterService = characterService;
            _worldService = worldService;
            _messagequeueService = messagequeueService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new MessagequeueViewmodel();
            model.Characters = new Dictionary<string, int>();

            var characters = _characterService.GetOnlineCharacters();
            foreach (var c in characters)
            {
                model.Characters.Add(c.Name, c.Id);
            }

            model.Worldsections = new Dictionary<string, int>();
            var ws = _worldService.GetMaps();
            foreach (var m in ws)
            {
                model.Worldsections.Add(m.Name, m.Id);
            }

            model.Queuemessages = _messagequeueService.GetMessagequeue();

            return View(model);
        }

        [HttpPost]
        public ActionResult Queue(MessagequeueViewmodel model)
        {
            switch (model.Type)
            {
                case Messagetype.Tele:
                    var location = new 
                    {
                        WorldsectionId = model.WorldsectionId,
                        X = model.DestinationX,
                        Y = model.DestinationY,
                        CharacterId = model.CharacterId
                    };
                    
                    var element = new Queueelement
                    {
                        Type = Messagetype.Tele,
                        Parameters = Newtonsoft.Json.JsonConvert.SerializeObject(location)
                    };

                    _messagequeueService.Push(element);
                    break;
            }
            return RedirectToAction("Index");
        }
    }

    public class MessagequeueViewmodel
    {
        public Messagetype Type { get; set; }
        public Dictionary<string, int> Characters { get; set; }
        public int CharacterId { get; set; }
        public int DestinationX { get; set; }
        public int DestinationY { get; set; }

        public Dictionary<string, int> Worldsections { get; set; }
        public int WorldsectionId { get; set; }
        public List<Queueelement> Queuemessages { get; set; }
    }
}