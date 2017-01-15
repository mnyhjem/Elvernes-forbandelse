using System.Collections.Generic;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.InteractiveObjects;
using ElvenCurse.Core.Model.Creatures.Npcs;

namespace ElvenCurse.Core.Interfaces
{
    public interface IWorldService
    {
        void EnterWorld(string getUserId);
        void LeaveWorld(string getUserId);
        Worldsection GetMap(int locationWorldsectionId);
        List<Worldsection> GetMaps();
        bool SaveMap(Worldsection modelWorldsection, string mapdata);
        List<NpcBase> GetAllNpcs();
        List<InteractiveObject> GetAllInteractiveObjects();
        List<Terrainfile> GetTerrains();
        Terrainfile GetTerrain(int id);
        bool SaveTerrain(Terrainfile model, string data);
    }
}
