using System.Net.Http;
using System.Text;
using System.Web.Http;
using ElvenCurse.Core.Interfaces;

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
            var map = _worldService.GetMap(id);

            return new HttpResponseMessage
            {
                Content = new StringContent(map.Json, Encoding.UTF8, "json/application")
            };
        }
    }
}