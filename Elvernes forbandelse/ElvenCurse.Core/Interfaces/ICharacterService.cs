using ElvenCurse.Core.Model;

namespace ElvenCurse.Core.Interfaces
{
    public interface ICharacterService
    {
        System.Collections.Generic.List<ElvenCurse.Core.Model.Character> GetCharactersForUser(string userId);
        bool CreateNewCharacter(string userId, Character model);
    }
}
