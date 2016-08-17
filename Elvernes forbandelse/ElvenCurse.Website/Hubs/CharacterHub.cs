using System.Threading.Tasks;
using System.Web;
using ElvenCurse.Core.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace ElvenCurse.Website.Hubs
{
    [Authorize]
    public class CharacterHub : Hub
    {
        private readonly IWorldService _worldService;
        private readonly ICharacterService _characterService;

        //public CharacterHub(
        //    IWorldService worldService,
        //    ICharacterService characterService)
        //{
        //    _worldService = worldService;
        //    _characterService = characterService;
        //}

        //public override Task OnConnected()
        //{
        //    _worldService.EnterWorld(HttpContext.Current.User.Identity.GetUserId());

        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    _worldService.LeaveWorld(HttpContext.Current.User.Identity.GetUserId());

        //    return base.OnDisconnected(stopCalled);
        //}

        public void Test()
        {
            Clients.All.hello("hejsa");
        }

        public void EnterWorldsection(int sectionId, int x, int y)
        {
            _characterService.SetCharacterPosition(sectionId, x, y);
            
            
            // Tilføj bruger til gruppen, som er vores sectionid
            Groups.Add(Context.ConnectionId, sectionId.ToString());

            Clients.OthersInGroup(sectionId.ToString()).javascriptmethode("Nogen kom ind i sectionen");

            Clients.Caller.javascriptmetode("vi skal vide at alle de andre er der..");
        }
    }
}