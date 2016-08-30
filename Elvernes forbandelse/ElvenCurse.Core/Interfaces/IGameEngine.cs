namespace ElvenCurse.Core.Interfaces
{
    public interface IGameEngine
    {
        void EnterWorld(string getUserId, string connectionId);
        void LeaveWorld(string connectionId);
        void EnterWorldsection(string userId, int sectionId, int x, int y);
        int Onlinecount { get; }
        void MovePlayer(string connectionId, string getUserId, int sectionId, int x, int y);
        void ChangeMap(string connectionId, string getUserId, string direction);

        void ClickOnInteractiveObject(string connectionId, string getUserId, int ioId);

        void SendToClientsInteractiveObjects(int worldsectionId, bool loadFromDatabase = false);
        void SendToClientsNpcs(int worldsectionId, bool loadFromDatabase = false);
    }
}
