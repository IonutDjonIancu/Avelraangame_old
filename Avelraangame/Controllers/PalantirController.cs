using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

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

        // POST: api/palantir/Logon
        [HttpPost("Logon")]
        public string Logon([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var playersService = new PlayersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = playersService.Logon(request);
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
        // GET: /api/palantir/GetCharactersByPlayerId
        [HttpGet("GetCharactersByPlayerId")]
        public string GetCharactersByPlayerId([FromQuery] RequestVm request)
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

        // GET: /api/palantir/GetAliveCharactersByPlayerId
        [HttpGet("GetAliveCharactersByPlayerId")]
        public string GetAliveCharactersByPlayerId([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var characterService = new CharactersService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = characterService.GetAliveCharactersByPlayer(request);
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
        //// GET: /api/palantir/Attack
        //[HttpGet("Attack")]
        //public string Attack([FromQuery] RequestVm request)
        //{
        //    var responseVm = new ResponseVm();
        //    var combatService = new CombatService();

        //    try
        //    {
        //        PalantirBase.ValidateRequest(request);
        //        responseVm.Data = combatService.Attack(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseVm.Error = ex.Message;
        //        return JsonConvert.SerializeObject(responseVm);
        //    }

        //    return JsonConvert.SerializeObject(responseVm);
        //}

        //// GET: /api/palantir/Defend
        //[HttpGet("Defend")]
        //public string Defend([FromQuery] RequestVm request)
        //{
        //    var responseVm = new ResponseVm();
        //    var combatService = new CombatService();

        //    try
        //    {
        //        PalantirBase.ValidateRequest(request);
        //        responseVm.Data = combatService.Defend(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseVm.Error = ex.Message;
        //        return JsonConvert.SerializeObject(responseVm);
        //    }

        //    return JsonConvert.SerializeObject(responseVm);
        //}

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

        // GET: /api/palantir/StartCombat
        [HttpGet("StartCombat")]
        public string StartCombat([FromQuery] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var combatService = new CombatService();

            try
            {
                responseVm.Data = combatService.StartCombat(request);
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
        // GET: /api/palantir/JoinParty
        [HttpPost("JoinParty")]
        public string JoinParty([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var combatService = new CombatService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = combatService.JoinParty(request);
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

        #region Episode
        #region GET
        // GET: /api/palantir/GetEpisodes
        [HttpGet("GetEpisodes")]
        public string GetEpisodes()
        {
            var responseVm = new ResponseVm();
            var episodeService = new EpisodesService();

            try
            {
                responseVm.Data = episodeService.GetEpisodes();
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
        // POST: /api/palantir/CRUDEpisode
        [HttpPost("CRUDEpisode")]
        public string CRUDEpisode([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var episodeService = new EpisodesService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = episodeService.EpisodeCRUD(request);
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

        #region Act
        #region GET
        #endregion

        #region POST
        // POST: /api/palantir/CRUDAct
        [HttpPost("CRUDAct")]
        public string CRUDAct([FromBody] RequestVm request)
        {
            var responseVm = new ResponseVm();
            var actService = new ActsService();

            try
            {
                PalantirBase.ValidateRequest(request);
                responseVm.Data = actService.ActCRUD(request);
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

        #region GameSettings
        #region GET
        // GET: /api/palantir/GetDifficulty
        [HttpGet("GetDifficulty")]
        public string GetDifficulty()
        {
            var responseVm = new ResponseVm();
            var actService = new ActsService();

            try
            {
                responseVm.Data = actService.GetDifficultyLevels();
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
