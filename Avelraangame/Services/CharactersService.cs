using Avelraangame.Models;
using Avelraangame.Models.POCOs;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avelraangame.Services
{
    public class CharactersService : CharacterSubService
    {
        #region Business logic

        public Guid JoinParty(Character chr)
        {
            if (chr.IsInFight)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is fighting."));
            }

            Party party;

            try
            {
                party = new Party()
                {
                    Id = new Guid(),
                    IsPartyLocked = false
                };
                DataService.CreateParty(party);

                chr.IsInParty = true;
                chr.PartyId = party.Id;
                DataService.UpdateCharacter(chr);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, $"{ex.Message}"));
            }

            return party.Id;
        }

        public void LeaveParty(Character chr)
        {
            if (chr.IsInFight)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is fighting."));
            }

            var party = DataService.GetPartyById(chr.PartyId.GetValueOrDefault());
            if (party == null)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.ResourceNotFound, "party not found."));
            }

            try
            {
                chr.IsInParty = false;
                chr.PartyId = null;
                DataService.UpdateCharacter(chr);

                var charactersLeft = DataService.GetCharactersByPartyId(chr.PartyId.GetValueOrDefault()).Count;

                if (charactersLeft <= 0)
                {
                    DataService.DeleteParty(party);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, $"{ex.Message}"));
            }
        }

        /// <summary>
        /// Adds stats roll and playerId to the newly created character, returns characterId.
        /// </summary>
        /// <param name="roll"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public string CreateCharacter(CharacterVm charVm)
        {
            var temps = new TempsService();

            var chr = CreateHumanCharacter(charVm);

            DataService.SaveCharacter(chr);

            temps.SaveTempCharInfo(chr.Id, TempUtils.BonusTo.Stats, charVm.Logbook.StatsRoll + (10 * GetEntityLevelByRoll(charVm.Logbook.StatsRoll)));
            temps.SaveTempCharInfo(chr.Id, TempUtils.BonusTo.Skills, 20);

            return string.Concat(Scribe.ShortMessages.Success);
        }

        public string SaveCharacterWithLevelUp(RequestVm request)
        {
            var charvm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            ValidateCharacterByPlayerId(charvm.CharacterId, charvm.PlayerId);
            var temps = new TempsService();

            // first get the unmodified character from db
            var chr = DataService.GetCharacterById(charvm.CharacterId);
            // get the temp data associated with the char
            var tempsInfo = temps.GetTempInfosByCharacterId(charvm.CharacterId);
            // compare and throw if necessary
            CompareTempsWithRequest(charvm, chr, tempsInfo);

            // set new values
            chr.Stats = JsonConvert.SerializeObject(charvm.Stats);
            chr.Skills = JsonConvert.SerializeObject(charvm.Skills);
            // remove levelup marker
            chr.HasLevelup = false;

            try
            {
                DataService.UpdateCharacter(chr);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, $": {ex.Message}."));
            }

            temps.RemoveTempsInfo(tempsInfo);

            return string.Concat(Scribe.ShortMessages.Success);
        }

        public (string responseMessage, string keyPlayerId, int roll) CharacterCreationRoll20(RequestVm request)
        {
            var charvm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            ValidatePlayerByIdNamePair(charvm.PlayerId, charvm.PlayerName);

            charvm.Logbook.StatsRoll = Dice.Roll_d_20();

            ValidateCharDiceBeforeReturn(charvm.PlayerId, charvm.Logbook.StatsRoll);

            return (JsonConvert.SerializeObject(charvm), charvm.PlayerId.ToString(), charvm.Logbook.StatsRoll);
        }

        public string EquipItem(RequestVm request)
        {
            var itmvm = ValidateRequestDeserializationIntoItemVm(request.Message);


            var items = new ItemsService();
            var item = ValidateItemByCharacterId(itmvm.Id, itmvm.CharacterId.GetValueOrDefault());
            var chr = DataService.GetCharacterById(itmvm.CharacterId.GetValueOrDefault());

            if (item.Type.Equals(ItemsUtils.Types.Valuables))
            {
                throw new Exception(string.Concat(Scribe.ShortMessages.Failure, ": cannot equip valuables, they can only be sold to a merchant."));
            }

            if (chr == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.Failure, ": character not found."));
            }

            item.InSlot = items.MoveItemInSlot(item.Type);
            // create an UnEquipItem for those that are already in occupied slot

            chr = items.EquipItemToSlot(chr, item);

            DataService.UpdateItem(item);
            DataService.UpdateCharacter(chr);

            var characterVm = GetCalculatedCharacter(chr.Id);

            return JsonConvert.SerializeObject(characterVm);
        }

        #endregion

        #region Public getters

        public string GetFame()
        {
            var allCharacters = DataService.GetCharacters().Where(s => s.IsAlive).ToList();
            var allCharactersVm = new List<CharacterVm>();

            foreach (var chr in allCharacters)
            {
                var charVm = new CharacterVm
                {
                    Name = chr.Name,
                    PlayerName = DataService.GetPlayerById(chr.PlayerId.GetValueOrDefault()).Name,
                    Expertise = JsonConvert.DeserializeObject<Expertise>(chr.Expertise),
                    Logbook = JsonConvert.DeserializeObject<Logbook>(chr.Logbook)
                };

                allCharactersVm.Add(charVm);
            }

            var result = new Fame
            {
                Fights = allCharactersVm.OrderByDescending(s => s.Logbook.Fights)
                    .ThenByDescending(s => s.Expertise.Experience)
                    .ThenBy(s => s.Logbook.EntityLevel)
                    .ThenBy(s => s.Logbook.StatsRoll)
                    .Take(5)
                    .ToList(),
                Wealth = allCharactersVm.OrderByDescending(s => s.Logbook.Wealth)
                    .ThenByDescending(s => s.Expertise.Experience)
                    .ThenBy(s => s.Logbook.EntityLevel)
                    .ThenBy(s => s.Logbook.StatsRoll)
                    .Take(5)
                    .ToList()
            };

            return JsonConvert.SerializeObject(result);
        }

        public new CharacterVm GenerateNPC(int minHitMarker, int maxHitMarker, Guid fightId)
        {
            return base.GenerateNPC(minHitMarker, maxHitMarker, fightId);
        }

        public string GetCalculatedCharacter(RequestVm request)
        {
            var reqCharVm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            ValidateCharacterByPlayerId(reqCharVm.CharacterId, reqCharVm.PlayerId);

            var charvm = GetCalculatedCharacter(reqCharVm.CharacterId);

            return JsonConvert.SerializeObject(charvm);
        }

        public CharacterVm GetCalculatedCharacterById(Guid characterId)
        {
            ValidateCharacterById(characterId);

            return GetCalculatedCharacter(characterId);
        }

        public string GetCharacterWithLevelUp(RequestVm request)
        {
            var reqCharVm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            ValidateCharacterByPlayerId(reqCharVm.CharacterId, reqCharVm.PlayerId);

            var charvm = new CharacterVm(DataService.GetCharacterById(reqCharVm.CharacterId));
            var temps = new TempsService();

            var tempBonuses = temps.GetTempInfosByCharacterId(reqCharVm.CharacterId);
            if (tempBonuses.Count.Equals(0))
            {
                return JsonConvert.SerializeObject(reqCharVm);
            }

            var listOfTempBonuses = new List<TempsVm>();

            foreach (var bonus in tempBonuses)
            {
                listOfTempBonuses.Add(new TempsVm(bonus));
            }

            charvm.Bonuses = listOfTempBonuses;

            return JsonConvert.SerializeObject(charvm);
        }

        public CharacterVm ValidateCharacter(Guid characterId, Guid playerId)
        {
            var character = ValidateCharacterByPlayerId(characterId, playerId);

            return new CharacterVm(character);
        }

        /// <summary>
        /// Gets the list of characters by a combination of player id and player name.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetCharactersByPlayer(RequestVm request)
        {
            var charvm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            ValidatePlayerByIdNamePair(charvm.PlayerId, charvm.PlayerName);

            var list = DataService.GetAllCharactersByPlayerId(charvm.PlayerId);

            var returnList = new List<CharacterVm>();

            foreach (var chr in list)
            {
                returnList.Add(new CharacterVm(chr));
            }

            var result = returnList.OrderBy(s => s.Name);

            return JsonConvert.SerializeObject(result);
        }

        public string GetAliveCharactersByPlayer(RequestVm request)
        {
            var charvm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            ValidatePlayerByIdNamePair(charvm.PlayerId, charvm.PlayerName);

            var list = DataService.GetAliveCharactersByPlayerId(charvm.PlayerId);

            var returnList = new List<CharacterVm>();

            foreach (var chr in list)
            {
                returnList.Add(new CharacterVm(chr));
            }

            var result = returnList.OrderBy(s => s.Name);

            return JsonConvert.SerializeObject(result);
        }

        #endregion
    }
}
