using Avelraangame.Models;
using Avelraangame.Models.POCOs;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.Base
{
    public class ServiceBase // move strings to Scribe
    {
        protected DataService DataService { get; set; }

        protected ServiceBase()
        {
            DataService = new DataService();
        }

        #region PlayerValidation
        protected PlayerVm ValidateRequestDeserializationIntoPlayerVm(string request)
        {
            PlayerVm playerVm;

            try
            {
                playerVm = JsonConvert.DeserializeObject<PlayerVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, $": {ex.Message}"));
            }

            return playerVm;
        }

        protected void ValidatePlayerId(Guid playerId)
        {
            if (playerId.Equals(Guid.Empty) || playerId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player id is missing or invalid."));
            }
        }

        protected Player ValidatePlayerById(Guid playerId)
        {
            ValidatePlayerId(playerId);

            var player = DataService.GetPlayerById(playerId);

            if (player == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": player not found."));
            }

            return player;
        }

        protected void ValidatePlayerName(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player name is missing or invalid."));
            }
        }

        protected Player ValidatePlayerByName(string playerName)
        {
            ValidatePlayerName(playerName);

            var player = DataService.GetPlayerByName(playerName);

            if (player == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": player not found."));
            }

            return player;
        }

        protected Player ValidatePlayerBySymbolWard(string symbol, string ward)
        {
            var player = DataService.GetPlayerBySymbolWard(symbol, ward);

            if (player == null)
            {
                throw new Exception(string.Concat(Scribe.ShortMessages.ResourceNotFound, ": the symbol ward matched no player."));
            }

            return player;
        }

        protected void ValidateWard(string ward, string wardCheck)
        {
            if (string.IsNullOrWhiteSpace(ward))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player ward is missing or invalid."));
            }

            if (ward.Equals(wardCheck)) { return; }

            throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": invalid ward."));
        }

        protected void ValidatePlayerByIdNamePair(Guid playerId, string playerName)
        {
            ValidatePlayerName(playerName);
            ValidatePlayerId(playerId);
            var player = ValidatePlayerById(playerId);

            if (player.Id.Equals(playerId)) { return; }
            
            throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": player name does not match its id."));
        }

        public bool IsPlayerValid(Guid playerid)
        {
            try
            {
                ValidatePlayerById(playerid);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        protected void ValidatePlayerUnicity(string name)
        {
            var playerExists = DataService.GetPlayersNames().Any(s => s.Contains(name));

            if (playerExists)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": player already exists."));
            }
        }

        protected void ValidateNumberOfPlayers()
        {
            var numOfPlayers = DataService.GetPlayersCount();

            if (numOfPlayers >= 100)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": number of player accounts reached."));
            }
        }

        protected void ValidatePlayerDetailsOnCreate(PlayerVm playerVm)
        {
            ValidatePlayerName(playerVm.PlayerName);
            ValidateWards(playerVm.Ward, playerVm.Wardcheck);
            if (!PlayersUtils.PlayerSymbolsList.Contains(playerVm.Symbol))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": invalid symbol."));
            }
        }

        protected void ValidatePlayerDetailsOnLogon(PlayerVm playerVm)
        {
            if (string.IsNullOrWhiteSpace(playerVm.Ward))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ward is missing or invalid."));
            }
            if (string.IsNullOrWhiteSpace(playerVm.Symbol))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": symbol is missing or invalid."));
            }
            if (!PlayersUtils.PlayerSymbolsList.Contains(playerVm.Symbol))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": invalid symbol."));
            }
        }

        private void ValidateWards(string ward, string wardCheck)
        {
            if (string.IsNullOrWhiteSpace(ward))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": ward is missing or invalid."));
            }

            if (string.IsNullOrWhiteSpace(wardCheck))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": wardcheck is missing or invalid."));
            }

            if (ward.Length > 4)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ward is too long, 4 characters maximum."));
            }

            if (!wardCheck.Equals(ward))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": wards don't match."));
            }
        }

        #endregion

        #region CharacterValidation
        protected CharacterVm ValidateRequestDeserializationInto_CharacterVm(string request)
        {
            CharacterVm charVm;

            try
            {
                charVm = JsonConvert.DeserializeObject<CharacterVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return charVm;
        }

        protected void ValidateCharVm(CharacterVm charVm)
        {
            ValidateCharacterId(charVm.CharacterId);
            ValidatePlayerId(charVm.PlayerId);
        }

        protected void ValidateCharacterId(Guid charId)
        {
            if (charId.Equals(Guid.Empty) || charId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": character id is missing or is invalid."));
            }
        }

        protected Character ValidateCharacterById(Guid charId)
        {
            ValidateCharacterId(charId);

            var chr = DataService.GetCharacterById(charId);

            if (chr == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": character not found."));
            }

            return chr;
        }

        protected Character ValidateCharacterByPlayerId(Guid charId, Guid playerId)
        {
            var chr = ValidateCharacterById(charId);

            if (chr.PlayerId.Equals(playerId)) { return chr; }

            throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character does not match player id."));
        }

        protected bool IsCharacterValid(Guid playerId, Guid charId)
        {
            try
            {
                ValidateCharacterByPlayerId(playerId, charId);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        protected string ValidateCharacterName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Scribe.Characters_GenericName;
            }

            return string.Concat(char.ToUpper(name[0]), name.Substring(1));
        }

        protected void ValidateCharDiceBeforeReturn(Guid playerId, int roll)
        {
            ValidatePlayerId(playerId);

            if (roll == 0)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character roll was 0"));
            }
        }

        public CharacterVm ValidateRollDetailsBeforeStoring(RequestVm request)
        {
            var charvm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            ValidatePlayerByIdNamePair(charvm.PlayerId, charvm.PlayerName);

            return charvm;
        }

        #endregion

        #region ItemValidation
        protected ItemVm ValidateRequestDeserializationIntoItemVm(string request)
        {
            ItemVm itemvm;

            try
            {
                itemvm = JsonConvert.DeserializeObject<ItemVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return itemvm;
        }

        protected void ValidateItemId(Guid itemId)
        {
            if (itemId.Equals(Guid.Empty) || itemId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": item id is missing or invalid."));
            }
        }

        protected Item ValidateItemById(Guid itemId)
        {
            ValidateItemId(itemId);

            var item = DataService.GetItemById(itemId);

            if (item == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, $": no item exists with id {itemId}."));
            }

            return item;
        }

        protected Item ValidateItemByCharacterId(Guid itemId, Guid characterId)
        {
            var item = ValidateItemById(itemId);

            if (item.CharacterId.Equals(characterId))
            {
                return item;
            }

            throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": item does not match character."));
        }

        #endregion

        #region CombatValidation
        public void ValidateAttackDetails(Attack attack)
        {
            if (attack == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": attack is missing or invalid."));
            }

            if (attack.FightId == Guid.Empty || attack.FightId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": fightId is missing or invalid."));
            }

            if (attack.Attacker == Guid.Empty || attack.Attacker == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": attacker is missing or invalid."));
            }

            if (attack.Defender == Guid.Empty || attack.Defender == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": defender is missing or invalid."));
            }
        }

        public Defend ValidateRequestDeserializationIntoDefend(string request)
        {
            Defend defend;

            try
            {
                defend = JsonConvert.DeserializeObject<Defend>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return defend;
        }

        public EndOfCombat ValidateRequestDeserializationIntoEndOfCombat(string request)
        {
            EndOfCombat combatEnd;

            try
            {
                combatEnd = JsonConvert.DeserializeObject<EndOfCombat>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return combatEnd;
        }

        protected Attack ValidateRequestDeserializationIntoAttack(string request)
        {
            Attack attack;

            try
            {
                attack = JsonConvert.DeserializeObject<Attack>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return attack;
        }
        protected CombatVm ValidateRequestDeserializationIntoCombatVm(string request)
        {
            CombatVm combatVm;

            try
            {
                combatVm = JsonConvert.DeserializeObject<CombatVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return combatVm;
        }

        #endregion

        #region EpisodeValidation
        protected EpisodeVm ValidateRequestDeserializationIntoEpisodeVm(string request)
        {
            EpisodeVm epiVm;

            try
            {
                epiVm = JsonConvert.DeserializeObject<EpisodeVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return epiVm;
        }

        protected void ValidateSigma(string sigmaWard)
        {
            if (string.IsNullOrEmpty(sigmaWard) ||
                !sigmaWard.Equals("206.453.sigma"))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure));
            }
        }

        protected void ValidateEpisodeName(string episodeName)
        {
            if (string.IsNullOrWhiteSpace(episodeName))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": episode name is missing or is invalid."));
            }
        }

        protected void ValidateEpisodeNameUnicity(string episodeName)
        {
            ValidateEpisodeName(episodeName);

            var episode = DataService.GetEpisodeByName(episodeName);

            if (episode == null)
            {
                return;
            }

            throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": episode with that name already exists."));
        }
        #endregion

        #region ActValidation
        protected void ValidateActName(string actName)
        {
            if (string.IsNullOrWhiteSpace(actName))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": act name is missing or is invalid."));
            }
        }

        protected void ValidateActNameUnicity(string actName)
        {
            ValidateActName(actName);

            var act = DataService.GetActByName(actName);

            if (act == null)
            {
                return;
            }

            throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": act with that name already exists."));
        }

        protected ActVm ValidateRequestDeserializationIntoActVm(string request)
        {
            ActVm actVm;

            try
            {
                actVm = JsonConvert.DeserializeObject<ActVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return actVm;
        }
        #endregion

        #region FightValidation
        protected List<CharacterVm> ValidateReqDeserializationInto_GoodGuys(string request)
        {
            List<CharacterVm> goodGuys;

            try
            {
                goodGuys = JsonConvert.DeserializeObject<List<CharacterVm>>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return goodGuys;
        }



        #endregion



    }
}
