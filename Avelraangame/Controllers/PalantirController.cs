using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Avelraangame.Controllers
{
    [Route("api/palantir")]
    [ApiController]
    public class PalantirController : Controller
    {
        // GET: api/palantir/GetOk
        [HttpGet("GetOk")]
        public string GetOk()
        {
            return Scribe.ShortMessages.Ok.ToString();
        }

        #region Items
        // GET: api/palantir/GenerateItem
        [HttpGet("GenerateItem")]
        public string GenerateItem()
        {
            var itemService = new ItemsService();
            return itemService.GenerateRandomItem();
        }

        // GET: api/palantir/GetItems
        [HttpGet("GetItemsCount")]
        public int GetItemsCount()
        {
            var itemService = new ItemsService();
            return itemService.GetItemsCount();
        }
        #endregion


        #region Players
        // POST: api/palantir/createplayer
        [HttpPost("CreatePlayer")]
        public string CreatePlayer([FromBody]RequestVm request)
        {
            var validateRequest = PalantirBase.ValidatePOSTRequest(request);

            if (validateRequest.Equals(Scribe.ShortMessages.BadRequest))
            {
                return validateRequest.ToString();
            }

            var playerService = new PlayersService();
            return playerService.CreatePlayer(request);
        }
        #endregion

        #region Characters
        // GET: api/palantir/CharacterRoll20
        [HttpGet("CharacterRoll20")]
        public string CharacterRoll20([FromQuery]RequestVm request)
        {
            var characterService = new CharactersService();

            var (responseMessage, playerId, roll) = characterService.CharacterRoll20(request);

            var storage = KeyValuePair.Create<string, object>(playerId, roll);

            if (!TempData.ContainsKey(playerId))
            {
                TempData.Add(storage);
            }

            return responseMessage;
        }

        // POST: api/palantir/createcharacter
        [HttpPost("CreateCharacter")]
        public string CreateCharacter([FromBody] RequestVm request)
        {
            var validateRequest = PalantirBase.ValidatePOSTRequest(request);

            if (validateRequest.Equals(Scribe.ShortMessages.BadRequest))
            {
                return validateRequest.ToString();
            }

            var characterService = new CharactersService();
            //return characterService.CreateCharacter(request);

            return "";
        }



        #endregion

    }
}
