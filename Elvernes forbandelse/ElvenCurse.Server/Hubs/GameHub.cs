using System.Security.Claims;
using System.Threading.Tasks;
using ElvenCurse.Core.Interfaces;
using Microsoft.AspNet.SignalR;
using ElvenCurse.Server.Extensions;

namespace ElvenCurse.Server.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IGameEngine _gameEngine;

        public GameHub(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public override Task OnConnected()
        {
            var userId = ((ClaimsPrincipal)Context.User).GetUserId();
            
            _gameEngine.EnterWorld(userId, Context.ConnectionId);
            Clients.All.onlinecount(_gameEngine.Onlinecount);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _gameEngine.LeaveWorld(Context.ConnectionId);
            Clients.All.onlinecount(_gameEngine.Onlinecount);

            return base.OnDisconnected(stopCalled);
        }

        public void Test()
        {
            Clients.All.hello("hejsa");
        }

        public void EnterWorldsection(int sectionId, int x, int y)
        {
            _gameEngine.EnterWorldsection(((ClaimsPrincipal)Context.User).GetUserId(), sectionId, x, y);
        }

        public void MovePlayer(int sectionId, int x, int y)
        {
            _gameEngine.MovePlayer(Context.ConnectionId, ((ClaimsPrincipal)Context.User).GetUserId(), sectionId, x, y);
        }

        public void ChangeMap(string direction)
        {
            _gameEngine.ChangeMap(Context.ConnectionId, ((ClaimsPrincipal)Context.User).GetUserId(), direction);
        }

        public void ClickOnInteractiveObject(int ioId)
        {
            _gameEngine.ClickOnInteractiveObject(Context.ConnectionId, ((ClaimsPrincipal)Context.User).GetUserId(), ioId);
        }

        public void ActivateAbility(int activatedAbility, int selectedCreatureId)
        {
            _gameEngine.ActivateAbility(Context.ConnectionId, ((ClaimsPrincipal)Context.User).GetUserId(), activatedAbility, selectedCreatureId);
        }
    }
}