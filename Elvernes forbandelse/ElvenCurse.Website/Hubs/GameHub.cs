using System.Threading.Tasks;
using System.Web;
using ElvenCurse.Core.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace ElvenCurse.Website.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IGameEngine _gameEngine;
        //private readonly IWorldService _worldService;
        //private readonly ICharacterService _characterService;

        //public CharacterHub(
        //    IWorldService worldService,
        //    ICharacterService characterService)
        //{
        //    _worldService = worldService;
        //    _characterService = characterService;
        //}

        public GameHub(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public override Task OnConnected()
        {
            _gameEngine.EnterWorld(HttpContext.Current.User.Identity.GetUserId(), Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (HttpContext.Current != null)
            {
                _gameEngine.LeaveWorld(HttpContext.Current.User.Identity.GetUserId(), Context.ConnectionId);
            }
            

            return base.OnDisconnected(stopCalled);
        }

        public void Onlinecount()
        {
            Clients.All.onlinecount(_gameEngine.Onlinecount);
        }

        public void Test()
        {
            Clients.All.hello("hejsa");
        }

        public void EnterWorldsection(int sectionId, int x, int y)
        {
            
            //Clients.OthersInGroup("test")
            //Context.
            //_characterService.SetCharacterPosition(sectionId, x, y);
            _gameEngine.EnterWorldsection(HttpContext.Current.User.Identity.GetUserId(), sectionId, x, y);
        }

        public void MovePlayer(int x, int y)
        {
            //Clients.OthersInGroup("test")
            //Context.
            //_characterService.SetCharacterPosition(sectionId, x, y);
            _gameEngine.MovePlayer(Context.ConnectionId, HttpContext.Current.User.Identity.GetUserId(), x, y);
        }
    }
}