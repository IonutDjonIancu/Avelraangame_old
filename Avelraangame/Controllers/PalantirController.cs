using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Avelraangame.Services.SubService;

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
            var responseVm = new ResponseVm();
            var itemService = new ItemsService();

            try
            {
                responseVm.Data = itemService.GenerateRandomItem();
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: api/palantir/GetItemsCount
        [HttpGet("GetItemsCount")]
        public string GetItemsCount()
        {
            var responseVm = new ResponseVm();
            var itemService = new ItemsService();

            try
            {
                responseVm.Data = itemService.GetItemsCount().ToString();
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
            }

            return JsonConvert.SerializeObject(responseVm);
        }
        #endregion


        #region Players
        // POST: api/palantir/createplayer
        [HttpPost("CreatePlayer")]
        public string CreatePlayer([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();

            var validateRequest = PalantirBase.ValidateRequest(request);

            if (!validateRequest.Equals(Scribe.ShortMessages.Ok))
            {
                responseVm.Error = validateRequest.ToString();
                return JsonConvert.SerializeObject(responseVm);
            }

            var playersService = new PlayersService();

            try
            {
                responseVm.Data = playersService.CreatePlayer(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
            }

            return JsonConvert.SerializeObject(responseVm);
        }
        
        // GET: api/palantir/GetAllPlayers
        [HttpGet("GetAllPlayers")]
        public string GetAllPlayers()
        {
            var responseVm = new ResponseVm();
            var playersService = new PlayersService();

            try
            {
                responseVm.Data = playersService.GetAllPlayers();
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }


        #endregion

        #region Characters
        // GET: api/palantir/Character_Roll20
        [HttpGet("Character_Roll20")]
        public string Character_Roll20([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();

            var validateRequest = PalantirBase.ValidateRequest(request);

            if (!validateRequest.Equals(Scribe.ShortMessages.Ok))
            {
                responseVm.Error = validateRequest.ToString();
                return JsonConvert.SerializeObject(responseVm);
            }

            var characterService = new CharactersService();
            (string response, string playerId, string roll) charRoll;

            try
            {
                charRoll = characterService.CreateCharacter_roll20(request);
                responseVm.Data = charRoll.response;
                var keyPlayerId = charRoll.playerId.ToString();
                if (!TempData.TryGetValue(keyPlayerId, out var value))
                {
                    TempData.Add(keyPlayerId, charRoll.roll);
                }
                else
                {
                    TempData.Remove(keyPlayerId);
                    TempData.Add(keyPlayerId, charRoll.roll);
                }

            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: api/palantir/Character_StoreRoll
        [HttpGet("Character_StoreRoll")]
        public string Character_StoreRoll([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var charService = new CharactersService();

            var validateRequest = PalantirBase.ValidateRequest(request);

            if (!validateRequest.Equals(Scribe.ShortMessages.Ok))
            {
                responseVm.Error = validateRequest.ToString();
                return JsonConvert.SerializeObject(responseVm);
            }

            CharacterVm charVm;

            try
            {
                charVm = JsonConvert.DeserializeObject<CharacterVm>(request.Message);

                var keyPlayerId = charVm.PlayerId.ToString();
                if (!TempData.TryGetValue(keyPlayerId, out var roll))
                {
                    throw new Exception(Scribe.ShortMessages.ResourceNotFound.ToString());
                }
                if (int.TryParse(roll.ToString(), out var rollValue))
                {
                    charVm.StatsRoll = rollValue;
                }

                responseVm.Data = JsonConvert.SerializeObject(charService.CreateCharacter_storeRoll(charVm));
                
                var keyPlayerIdName = string.Concat(charVm.PlayerId, charVm.PlayerName);
                if (!TempData.TryGetValue(keyPlayerIdName, out var value))
                {
                    TempData.Add(keyPlayerIdName, rollValue);
                }
                else
                {
                    TempData.Remove(keyPlayerIdName);
                    TempData.Add(keyPlayerIdName, rollValue);
                }
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }


        // GET: api/palantir/Character_AddCharacter
        [HttpGet("Character_AddCharacter")]
        public string Character_AddCharacter([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var characterService = new CharactersService();

            var validateRequest = PalantirBase.ValidateRequest(request);

            if (!validateRequest.Equals(Scribe.ShortMessages.Ok))
            {
                responseVm.Error = validateRequest.ToString();
                return JsonConvert.SerializeObject(responseVm);
            }

            CharacterVm charVm;
            var charId = Guid.Empty;

            try
            {
                charVm = JsonConvert.DeserializeObject<CharacterVm>(request.Message);
            }
            catch (Exception)
            {

                responseVm.Error = Scribe.ShortMessages.BadRequest.ToString();
                return JsonConvert.SerializeObject(responseVm);
            }

            try
            {
                var keyPlayerIdName = string.Concat(charVm.PlayerId.ToString(), charVm.PlayerName);

                if (TempData.TryGetValue(keyPlayerIdName, out var value))
                {
                    int.TryParse(value.ToString(), out var rollValue);

                    charVm.StatsRoll = rollValue;

                    //charId = characterService.CreateCharacter_step1(charVm);
                }

                responseVm.Data = charId.ToString();
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        




        

       



        #endregion



    }
}
