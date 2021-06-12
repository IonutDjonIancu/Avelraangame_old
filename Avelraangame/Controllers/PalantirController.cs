using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Avelraangame.Controllers
{
    [Route("api/palantir")]
    [ApiController]
    public class PalantirController : Controller
    {
        // GET: api/palantir/GetOk
        [HttpGet("GetOk")]
        public ResponseVm GetOk()
        {
            var player = new PlayerVm
            {
                Name = "player name",
                Ward = "1234",
                Wardcheck = "12345"
            };


            var response = new ResponseVm
            {
                Data = JsonConvert.SerializeObject(player),
                Error = Scribe.ShortMessages.Failure.ToString()
            };

            return response;
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
        public ResponseVm CreatePlayer([FromBody]RequestVm request)
        {
            var response = new ResponseVm();
            var validateRequest = PalantirBase.ValidatePOSTRequest(request);

            if (validateRequest.Equals(Scribe.ShortMessages.BadRequest))
            {
                response.Error = validateRequest.ToString();
                return response;
            }

            var playerService = new PlayersService();
            response.Data = playerService.CreatePlayer(request);

            return response;
        }
        #endregion

        #region Characters
        // GET: api/palantir/CharacterRoll20
        [HttpGet("CharacterRoll20")]
        public ResponseVm CharacterRoll20([FromQuery]RequestVm request)
        {
            var response = new ResponseVm();
            var characterService = new CharactersService();

            var (responseMessage, playerId, roll) = characterService.CharacterRoll20(request);

            if (playerId == null || roll == 0)
            {
                response.Error = responseMessage;
                
                return response;
            }

            var storage = KeyValuePair.Create<string, object>(playerId, roll);
            
            if (!TempData.ContainsKey(playerId))
            {
                TempData.Add(storage);
            }
            else
            {
                TempData.Remove(playerId);
                TempData.Add(storage);
            }

            response.Data = responseMessage;

            return response;
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
