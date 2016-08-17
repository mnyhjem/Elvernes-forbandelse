using System.Web.Http;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using Microsoft.AspNet.Identity;

namespace ElvenCurse.Website.api
{
    [Authorize]
    public class CharacterController : ApiController
    {
        private readonly ICharacterService _characterService;
        
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public Character Getactive()
        {
            var character = _characterService.GetOnlineCharacter(User.Identity.GetUserId());
            
            return character;
        }
    }
}
