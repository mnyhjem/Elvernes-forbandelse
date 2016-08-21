using System.Collections.Generic;
using System.Linq;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using Microsoft.AspNet.SignalR.Hubs;

namespace ElvenCurse.Core.Engines
{
    public class GameEngine : IGameEngine
    {
        private readonly IHubConnectionContext<dynamic> _clients;
        private readonly ICharacterService _characterService;
        private List<Character> _characters;

        public int Onlinecount
        {
            get { return _characters.Count; }
        }
        
        public GameEngine(IHubConnectionContext<dynamic> clients, ICharacterService characterService)
        {
            _clients = clients;
            _characterService = characterService;
            _characters = new List<Character>();
        }

        public void EnterWorld(string getUserId, string connectionId)
        {
            var c = _characterService.GetOnlineCharacter(getUserId);
            c.ConnectionId = connectionId;
            _characters.Add(c);
        }

        public void LeaveWorld(string getUserId, string connectionId)
        {
            _characters.Remove(_characters.FirstOrDefault(a => a.ConnectionId == connectionId));
        }

        public void EnterWorldsection(string userId, int sectionId, int x, int y)
        {
            //// Tilføj bruger til gruppen, som er vores sectionid
            //Groups.Add(Context.ConnectionId, sectionId.ToString());

            //_clients.

            //_clients.OthersInGroup(sectionId.ToString()).javascriptmethode("Nogen kom ind i sectionen");

            //Clients.Caller.javascriptmetode("vi skal vide at alle de andre er der..");
            //_clients.Groups("test");

            var grp = _clients.Group("test");

            //throw new System.NotImplementedException();
        }

        public void MovePlayer(string connectionId, string getUserId, int x, int y)
        {
            var c = _characters.FirstOrDefault(a => a.ConnectionId == connectionId);
            if (c == null)
            {
                return;
            }

            c.Location.X = x;
            c.Location.Y = y;

            // todo send opdatering til klienterne
            _clients.AllExcept(connectionId).updatePlayer(c);
        }
    }
}
