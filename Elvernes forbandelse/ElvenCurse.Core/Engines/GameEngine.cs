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
        private readonly IWorldService _worldService;
        private List<Character> _characters;

        public int Onlinecount
        {
            get { return _characters.Count; }
        }
        
        public GameEngine(
            IHubConnectionContext<dynamic> clients, 
            ICharacterService characterService,
            IWorldService worldService)
        {
            _clients = clients;
            _characterService = characterService;
            _worldService = worldService;
            _characters = new List<Character>();
        }

        public void EnterWorld(string getUserId, string connectionId)
        {
            var c = _characterService.GetOnlineCharacter(getUserId);
            c.ConnectionId = connectionId;
            
            var foundPlayer = _characters.FirstOrDefault(a => a.Id == c.Id);
            if (foundPlayer == null)
            {
                _characters.Add(c);
            }
            else
            {
                foundPlayer.ConnectionId = connectionId;
            }
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

        public void MovePlayer(string connectionId, string getUserId, int sectionId, int x, int y)
        {
            var c = _characters.FirstOrDefault(a => a.ConnectionId == connectionId);
            if (c == null)
            {
                return;
            }

            //if (c.Location.WorldsectionId != sectionId)
            //{
            //    _clients.AllExcept(connectionId).updatePlayer(c);
            //}

            c.Location.X = x;
            c.Location.Y = y;
            c.Location.WorldsectionId = sectionId;

            _clients.AllExcept(connectionId).updatePlayer(c);
        }

        public void ChangeMap(string connectionId, string getUserId, string direction)
        {
            var c = _characters.FirstOrDefault(a => a.ConnectionId == connectionId);
            if (c == null)
            {
                return;
            }

            var currentMap = _worldService.GetMap(c.Location.WorldsectionId);
            var newPlayerlocationSuccess = new Location
            {
                WorldsectionId = c.Location.WorldsectionId,
                X = c.Location.X,
                Y = c.Location.Y
            };

            var newPlayerlocationFailed = new Location
            {
                WorldsectionId = c.Location.WorldsectionId,
                X = c.Location.X,
                Y = c.Location.Y
            };

            var mapToLoad = 0;
            switch (direction)
            {
                case "left":
                    mapToLoad = currentMap.MapchangeLeft;
                    newPlayerlocationSuccess.X = 99;
                    newPlayerlocationSuccess.Y = -1;

                    newPlayerlocationFailed.X -= 1;
                    newPlayerlocationFailed.Y = -1;
                    break;

                case "right":
                    mapToLoad = currentMap.MapchangeRight;
                    newPlayerlocationSuccess.X = 1;
                    newPlayerlocationSuccess.Y = -1;

                    newPlayerlocationFailed.X = 99;
                    newPlayerlocationFailed.Y = -1;
                    break;

                case "up":
                    mapToLoad = currentMap.MapchangeUp;
                    newPlayerlocationSuccess.X = -1;
                    newPlayerlocationSuccess.Y = 99;

                    newPlayerlocationFailed.X = -1;
                    newPlayerlocationFailed.Y -= 1;
                    break;

                case "down":
                    mapToLoad = currentMap.MapchangeDown;
                    newPlayerlocationSuccess.X = -1;
                    newPlayerlocationSuccess.Y = 1;

                    newPlayerlocationFailed.X = -1;
                    newPlayerlocationFailed.Y = 99;
                    break;

                case "playerposition":
                    mapToLoad = currentMap.Id;
                    break;
            }
            
            var map = _worldService.GetMap(mapToLoad);
            if (map != null)
            {
                c.Location = newPlayerlocationSuccess;
                c.Location.WorldsectionId = mapToLoad;
                for (var i = 0; i < map.Tilemap.layers.Count; i++)
                {
                    map.Tilemap.layers[i].data = null;
                }
            }
            else
            {
                c.Location = newPlayerlocationFailed;
            }

            _clients.Client(connectionId).changeMap(map);


            _clients.Client(connectionId).updateOwnPlayer(c);
        }
    }
}
