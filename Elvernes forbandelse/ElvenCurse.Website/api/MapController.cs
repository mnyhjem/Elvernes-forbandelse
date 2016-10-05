using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;

namespace ElvenCurse.Website.api
{
    [Authorize]
    public class MapController : ApiController
    {
        private readonly IWorldService _worldService;

        public MapController(IWorldService worldService)
        {
            _worldService = worldService;
        }

        [HttpGet]
        public HttpResponseMessage GetMap(int id)
        {
            var terrains = new List<Terrainfile>();
            var map = _worldService.GetMap(id);

            if (map.Tilemap.HasTerrainreferences)
            {
                terrains = _worldService.GetTerrains();
            }

            var jsondata = map.Tilemap.GetJson(terrains);


            return new HttpResponseMessage
            {
                Content = new StringContent(jsondata, Encoding.UTF8, "json/application")
                //Content = new StringContent("bøf", Encoding.UTF8, "json/application")
            };
        }
    }
}