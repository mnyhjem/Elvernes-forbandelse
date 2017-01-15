using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Creatures.Npcs;
using ElvenCurse.Core.Model.InteractiveObjects;
using ElvenCurse.Core.Utilities;
using Microsoft.AspNet.SignalR.Hubs;

namespace ElvenCurse.Core.Engines
{
    public class GameEngine : IGameEngine
    {
        private Random _rnd = new Random();

        private readonly IHubConnectionContext<dynamic> _clients;
        private readonly ICharacterService _characterService;
        private readonly IWorldService _worldService;
        private List<Character> _characters;
        private List<Worldsection> _worldsections;
        private List<NpcBase> _npcs;
        private List<InteractiveObject> _interactiveObjects;

        private Timer _timer;
        private readonly TimeSpan _timerUpdateInterval = TimeSpan.FromMilliseconds(250);
        private readonly object _timerUpdateLock = new object();
        private volatile bool _updatingTimer;

        private readonly DateTime _serverBoottime;

        public int Onlinecount
        {
            get { return _characters.Count; }
        }

        private dynamic AllInWorldSectionExceptCurrent(Character c)
        {
            return
                _clients.Clients(
                    _characters.Where(a => a.Location.WorldsectionId == c.Location.WorldsectionId && a.Id != c.Id)
                        .Select(a => a.ConnectionId)
                        .ToList());
        }

        private dynamic AllInWorldSection(int sectionId)
        {
            return
                _clients.Clients(
                    _characters.Where(a => a.Location.WorldsectionId == sectionId)
                        .Select(a => a.ConnectionId)
                        .ToList());
        }

        public GameEngine(
            IHubConnectionContext<dynamic> clients, 
            ICharacterService characterService,
            IWorldService worldService)
        {
            _serverBoottime = DateTime.Now;
            Trace.WriteLine($"Server bootup at {_serverBoottime}");
            
            _clients = clients;
            _characterService = characterService;
            _worldService = worldService;
            _characters = new List<Character>();
            _worldsections = new List<Worldsection>();
            _npcs = _worldService.GetAllNpcs();
            _interactiveObjects = _worldService.GetAllInteractiveObjects();

            _timer = new Timer(TimerTick, null, _timerUpdateInterval, _timerUpdateInterval);
        }

        private Worldsection GetWorldsection(int sectionId, bool getReferenceObject = false)
        {
            var section = _worldsections.FirstOrDefault(a => a.Id == sectionId);
            if (section == null)
            {
                section = _worldService.GetMap(sectionId);
                if (section == null)
                {
                    // dette kort findes ikke..
                    return null;
                }
                _worldsections.Add(section);
            }

            if (getReferenceObject)
            {
                return section;
            }
            return ExtensionsAndUtilities.DeepCopy(section);
        }

        public void EnterWorld(string getUserId, string connectionId)
        {
            var c = _characterService.GetOnlineCharacter(getUserId);
            c.ConnectionId = connectionId;
            
            var foundPlayer = _characters.FirstOrDefault(a => a.Id == c.Id);
            if (foundPlayer == null)
            {
                foundPlayer = c;
                _characters.Add(c);
                Trace.WriteLine($"{foundPlayer.Name} entered the world");
            }
            else
            {
                foundPlayer.ConnectionId = connectionId;
                Trace.WriteLine($"{foundPlayer.Name} reconnected to the world");
            }

            foundPlayer.Connectionstatus = Connectionstatus.Online;
        }

        public void LeaveWorld(string connectionId)
        {
            var c = _characters.FirstOrDefault(a => a.ConnectionId == connectionId);
            if (c != null)
            {
                c.Connectionstatus = Connectionstatus.Offline;
                _characters.Remove(c);

                AllInWorldSectionExceptCurrent(c).updatePlayer(c);

                _characterService.SavePlayerinformation(c);

                Trace.WriteLine($"{c.Name} left the world");
            }
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
            
            c.Location.X = x;
            c.Location.Y = y;
            c.Location.WorldsectionId = sectionId;

            var currentmap = GetWorldsection(c.Location.WorldsectionId);

            if (c.Location.X < 1)
            {
                //this.gameHub.server.changeMap("left");
                ChangeMap(connectionId, getUserId, "left");
                return;
            }
            if (c.Location.X >= currentmap.Tilemap.Width)
            {
                //this.gameHub.server.changeMap("right");
                ChangeMap(connectionId, getUserId, "right");
                return;
            }
            if (c.Location.Y > currentmap.Tilemap.Height)
            {
                //this.gameHub.server.changeMap("down");
                ChangeMap(connectionId, getUserId, "down");
                return;
            }
            if (c.Location.Y < 1)
            {
                //this.gameHub.server.changeMap("up");
                ChangeMap(connectionId, getUserId, "up");
                return;
            }

            AllInWorldSectionExceptCurrent(c).updatePlayer(c);
        }

        public void ChangeMap(string connectionId, string getUserId, string direction)
        {
            var c = _characters.FirstOrDefault(a => a.ConnectionId == connectionId);
            if (c == null)
            {
                return;
            }

            var currentMap = GetWorldsection(c.Location.WorldsectionId);
            var newPlayerlocationSuccess = new Location
            {
                WorldsectionId = c.Location.WorldsectionId,
                X = c.Location.X,
                Y = c.Location.Y
            };

            var oldPlayerLocation = new Location
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

                    oldPlayerLocation.X -= 1;
                    oldPlayerLocation.Y = -1;
                    break;

                case "right":
                    mapToLoad = currentMap.MapchangeRight;
                    newPlayerlocationSuccess.X = 1;
                    newPlayerlocationSuccess.Y = -1;

                    oldPlayerLocation.X = 99;
                    oldPlayerLocation.Y = -1;
                    break;

                case "up":
                    mapToLoad = currentMap.MapchangeUp;
                    newPlayerlocationSuccess.X = -1;
                    newPlayerlocationSuccess.Y = 99;

                    oldPlayerLocation.X = -1;
                    oldPlayerLocation.Y -= 1;
                    break;

                case "down":
                    mapToLoad = currentMap.MapchangeDown;
                    newPlayerlocationSuccess.X = -1;
                    newPlayerlocationSuccess.Y = 1;

                    oldPlayerLocation.X = -1;
                    oldPlayerLocation.Y = 99;
                    break;

                case "playerposition":
                    mapToLoad = currentMap.Id;
                    break;
            }
            
            var map = GetWorldsection(mapToLoad);
            if (map != null)
            {
                c.Location = newPlayerlocationSuccess;
                c.Location.WorldsectionId = mapToLoad;
                for (var i = 0; i < map.Tilemap.Layers.Length; i++)
                {
                    map.Tilemap.Layers[i].Data = null;
                }

                // fortæl spillerne i den section vi forlader, at vi er taget afsted
                AllInWorldSection(oldPlayerLocation.WorldsectionId).updatePlayer(c);

                if (map.Tilemap.HasTerrainreferences)
                {
                    // vores javascript klient har det dårligt med terrain referencer, så vi erstatter disse med de rigtige tilesets..
                    var terrains = _worldService.GetTerrains();
                    if (terrains != null)
                    {
                        for (int index = 0; index < map.Tilemap.Tilesets.Length; index++)
                        {
                            var tileset = map.Tilemap.Tilesets[index];
                            if (tileset.IsTerrainreference)
                            {
                                var correctTileset = terrains.FirstOrDefault(a => string.Equals(a.Filename, tileset.Source, StringComparison.CurrentCultureIgnoreCase));
                                if (correctTileset != null)
                                {
                                    map.Tilemap.Tilesets[index] = correctTileset.Tileset;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                c.Location = oldPlayerLocation;
            }

            // indlæs kort mm.
            _clients.Client(connectionId).changeMap(map);
            
            // placer vores egen spiller på kortet
            _clients.Client(connectionId).updateOwnPlayer(c);

            // fortæl de andre spillere at vi er kommet
            AllInWorldSectionExceptCurrent(c).updatePlayer(c);

            // placer de andre spillere på kortet
            foreach (var otherplacer in _characters.Where(a => a.Location.WorldsectionId == c.Location.WorldsectionId && a.ConnectionId != connectionId))
            {
                _clients.Client(connectionId).updatePlayer(otherplacer);
            }

            // placer npcere på kortet
            foreach (var npc in _npcs.Where(a => a.CurrentLocation.WorldsectionId == c.Location.WorldsectionId))
            {
                _clients.Client(connectionId).updateNpc(npc.ToIPlayer());
            }

            // placer interactiveobjects på kortet
            _clients.Client(connectionId).updateInteractiveObjects(_interactiveObjects.Where(a => a.Location.WorldsectionId == c.Location.WorldsectionId));
        }

        public void ClickOnInteractiveObject(string connectionId, string getUserId, int ioId)
        {
            var c = _characters.FirstOrDefault(a => a.ConnectionId == connectionId);
            if (c == null)
            {
                return;
            }

            // find objektet..
            var io = _interactiveObjects.FirstOrDefault(a => a.Id == ioId);
            if (io == null)
            {
                return;
            }

            var result = io.Interact(c);
            switch (result)
            {
                case InteractiveobjectResult.ChangeUsersMap:
                    ChangeMap(connectionId, getUserId, "playerposition");
                    break;
            }
        }

        public void SendToClientsInteractiveObjects(int worldsectionId, bool loadFromDatabase = false)
        {
            if (loadFromDatabase)
            {
                _interactiveObjects = _worldService.GetAllInteractiveObjects();
            }

            _clients.Clients(_characters
                .Where(a => a.Location.WorldsectionId == worldsectionId)
                .Select(a => a.ConnectionId).ToList())
                .updateInteractiveObjects(_interactiveObjects.Where(a => a.Location.WorldsectionId == worldsectionId));
        }

        public void SendToClientsNpcs(int worldsectionId, bool loadFromDatabase = false)
        {
            if (loadFromDatabase)
            {
                _npcs = _worldService.GetAllNpcs();
            }

            foreach (var npc in _npcs.Where(a => a.CurrentLocation.WorldsectionId == worldsectionId))
            {
                _clients.Clients(_characters
                .Where(a => a.Location.WorldsectionId == worldsectionId)
                .Select(a => a.ConnectionId).ToList())
                .updateNpc(npc.ToIPlayer());
            }
        }

        private void TimerTick(object state)
        {
            lock (_timerUpdateLock)
            {
                if (!_updatingTimer)
                {
                    _updatingTimer = true;

                    // update
                    foreach (var npc in _npcs)
                    {
                        npc.Move(_characters);

                        npc.ProcessAffectedby();

                        if (npc.UpdateNeeded)
                        {
                            AllInWorldSection(npc.CurrentLocation.WorldsectionId).updateNpc(npc.ToIPlayer());
                        }
                    }

                    // kør affected by for alle characters
                    foreach (var c in _characters)
                    {
                        c.ProcessAffectedby();

                        if (c.UpdateNeeded)
                        {
                            //AllInWorldSection(c.Location.WorldsectionId).updatePlayer(c);
                            // opdater vores egen spiller på kortet
                            var ownPlayer = _characters.FirstOrDefault(a => a.Id == c.Id);
                            if (ownPlayer != null)
                            {
                                _clients.Client(ownPlayer.ConnectionId).updateOwnPlayer(c);
                            }
                            
                            // fortæl de andre spillere at vi er kommet
                            AllInWorldSectionExceptCurrent(c).updatePlayer(c);
                        }
                    }

                    _updatingTimer = false;
                }
            }
        }
    }
}
