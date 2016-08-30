using ElvenCurse.Core.Model;

namespace ElvenCurse.Core.Interfaces
{
    public interface ICharacterService
    {
        System.Collections.Generic.List<Character> GetCharactersForUser(string userId);
        bool CreateNewCharacter(string userId, Character model);
        Character GetCharacter(string userId, int characterId);
        void SetCharacterOnline(string userId, int selectedCharacterId);
        Character GetOnlineCharacter(string getUserId);
        void SavePlayerinformation(Character character);
    }
}
