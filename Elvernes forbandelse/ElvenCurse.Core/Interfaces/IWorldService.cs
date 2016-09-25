using System.Collections.Generic;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.InteractiveObjects;
using ElvenCurse.Core.Model.Npcs;

namespace ElvenCurse.Core.Interfaces
{
    public interface IWorldService
    {
        IEnumerable<Character> GetOnlineCharacters();
        void EnterWorld(string getUserId);
        void LeaveWorld(string getUserId);
        Worldsection GetMap(int locationWorldsectionId);
        List<Worldsection> GetMaps();
        bool SaveMap(Worldsection modelWorldsection);
        List<Npc> GetAllNpcs();
        List<InteractiveObject> GetAllInteractiveObjects();
    }
}
