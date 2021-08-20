using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Avelraangame.Services.SubService;
using Avelraangame.Models.ModelScraps;

namespace Avelraangame.Controllers
{
    [Route("api/palantir")]
    [ApiController]
    public class PalantirController : Controller
    {
        // GET: /api/palantir/GetOk
        [HttpGet("GetOk")]
        public string GetOk()
        {
            return Scribe.ShortMessages.Ok.ToString();
        }

        #region Players
        #region GET
        // GET: api/palantir/GetPlayersNames
        [HttpGet("GetPlayersNames")]
        public string GetPlayersNames()
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

        // GET: api/palantir/GetPlayerIdByName
        [HttpGet("GetPlayerIdByName")]
        public string GetPlayerIdByName([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var playersService = new PlayersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = playersService.GetPlayerIdByName(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }
        #endregion
        #region POST
        // POST: api/palantir/CreatePlayer
        [HttpPost("CreatePlayer")]
        public string CreatePlayer([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var playersService = new PlayersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = playersService.CreatePlayer(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }
        #endregion
        #endregion

        #region Characters
        #region GET
        // GET: /api/palantir/GetCharactersByPlayer
        [HttpGet("GetCharactersByPlayer")]
        public string GetCharactersByPlayer([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var characterService = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = characterService.GetCharactersByPlayer(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/StoreRoll
        [HttpGet("StoreRoll")]
        public string StoreRoll([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var charactersService = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                var charvm = charactersService.ValidateRollDetailsBeforeStoring(request);
                var keyPlayerId = charvm.PlayerId.ToString();
                
                if (!TempData.TryGetValue(keyPlayerId, out var roll))
                {
                    throw new Exception(Scribe.ShortMessages.ResourceNotFound.ToString());
                }
            
                var keyPlayerIdName = string.Concat(charvm.PlayerId, charvm.PlayerName);

                if (!TempData.TryGetValue(keyPlayerIdName, out var value))
                {
                    TempData.Add(keyPlayerIdName, roll);
                }
                else
                {
                    TempData.Remove(keyPlayerIdName);
                    TempData.Add(keyPlayerIdName, roll);
                }

                responseVm.Data = roll.ToString();

            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/CharacterCreationRoll20
        [HttpGet("CharacterCreationRoll20")]
        public string CharacterCreationRoll20([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var charactersService = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                var (responseMessage, keyPlayerId, roll) = charactersService.CharacterCreationRoll20(request);
                responseVm.Data = responseMessage;

                if (!TempData.TryGetValue(keyPlayerId, out var _))
                {
                    TempData.Add(keyPlayerId, roll);
                }
                else
                {
                    TempData.Remove(keyPlayerId);
                    TempData.Add(keyPlayerId, roll);
                }

            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/GetCharacter
        [HttpGet("GetCharacter")]
        public string GetCharacter([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var charactersService = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = charactersService.GetCalculatedCharacter(request);
            }
            catch (Exception ex)
            {

                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/GetCharacterLevelup
        [HttpGet("GetCharacterLevelup")]
        public string GetCharacterLevelup([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var charactersService = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = charactersService.GetCharacterWithLevelUp(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }
        #endregion
        #region POST
        // POST: /api/palantir/CreateCharacter
        [HttpPost("CreateCharacter")]
        public string CreateCharacter([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var characterService = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                var charvm = JsonConvert.DeserializeObject<CharacterVm>(request.Message);
                var keyPlayerIdName = string.Concat(charvm.PlayerId.ToString(), charvm.PlayerName);

                if (TempData.TryGetValue(keyPlayerIdName, out var value))
                {
                    int.TryParse(value.ToString(), out var rollValue);

                    charvm.Logbook.StatsRoll = rollValue;

                    responseVm.Data = characterService.CreateCharacter(charvm);
                }
                else
                {
                    throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": "));
                }
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // POST: /api/palantir/LevelupCharacter
        [HttpPost("LevelupCharacter")]
        public string LevelupCharacter([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var characters = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = characters.SaveCharacterWithLevelUp(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // POST: /api/palantir/EquipItem
        [HttpPost("EquipItem")]
        public string EquipItem([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var characterService = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = characterService.EquipItem(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        #endregion
        #endregion

        #region Items
        #region GET
        // GET: /api/palantir/SellItem
        [HttpGet("SellItem")]
        public string SellItem([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var itemService = new ItemsService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = itemService.SellItem(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/GetItemsByCharacter
        [HttpGet("GetItemsByCharacter")]
        public string GetItemsByCharacter([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var itemService = new ItemsService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = itemService.GetItemsByCharacter(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: api/palantir/GenerateItem
        [HttpGet("GenerateItem")]
        public string GenerateItem()
        {
            var responseVm = new ResponseVm();
            var itemService = new ItemsService();

            try
            {
                responseVm.Data = JsonConvert.SerializeObject(itemService.GenerateRandomItem());
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
        #region POST
        #endregion
        #endregion

        #region Combat
        #region GET
        // GET: /api/palantir/Attack
        [HttpGet("Attack")]
        public string Attack([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var combatService = new CombatService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = combatService.Attack(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/Defend
        [HttpGet("Defend")]
        public string Defend([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var combatService = new CombatService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = combatService.Defend(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/EndCombat
        [HttpGet("EndCombat")]
        public string EndCombat([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var combatService = new CombatService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = combatService.EndCombat(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/GetFight
        [HttpGet("GetFight")]
        public string GetFight([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var combatService = new CombatService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = combatService.GetFight(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }

        // GET: /api/palantir/GenerateWeakNpcFight
        [HttpGet("GenerateWeakNpcFight")]
        public string GenerateWeakNpcFight([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var combatService = new CombatService();

            try
            {
                responseVm.Data = combatService.GenerateWeakNpcFight(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }


        #endregion
        #region POST
        // GET: /api/palantir/GoToParty
        [HttpPost("GoToParty")]
        public string GoToParty([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var combatService = new CombatService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = combatService.GoToParty(request);
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }


        #endregion
        #endregion

        #region Home
        #region GET
        // GET: api/palantir/GetFame
        [HttpGet("GetFame")]
        public string GetFame()
        {
            var responseVm = new ResponseVm();
            var characterService = new CharactersService();

            try
            {
                responseVm.Data = characterService.GetFame();
            }
            catch (Exception ex)
            {
                responseVm.Error = ex.Message;
                return JsonConvert.SerializeObject(responseVm);
            }

            return JsonConvert.SerializeObject(responseVm);
        }


        #endregion
        #endregion
    }
}
