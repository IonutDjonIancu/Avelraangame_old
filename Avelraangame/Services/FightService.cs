using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Avelraangame.Services
{
    public class FightService : FightSubService
    {
        #region BusinessLogic
        public FightVm StartAFight(CharacterVm requester)
        {
            var fightId = Guid.NewGuid();
            var partyCharacters = new List<Character>();
            var fight = new Fight();
            var fightvm = new FightVm();

            try
            {
                var chrRequester = ValidateCharacterByPlayerId(requester.CharacterId, requester.PlayerId);

                var party = DataService.GetPartyById(chrRequester.PartyId.GetValueOrDefault());
                if (party == null)
                {
                    throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "party does not exist."));
                }

                partyCharacters = DataService.GetCharactersByPartyId(party.Id);
                LockCharactersInFight(partyCharacters, fightId);

                var (goodGuys, minHitMarker, maxHitMarker) = GetTheGoodGuys(partyCharacters);
                var (badGuys, partySize) = GetTheBadGuys(minHitMarker, maxHitMarker, fightId);
                var fightDetails = GetTheFightDetails(badGuys, partySize);

                fight.Id = fightId;
                fight.GoodGuys = JsonConvert.SerializeObject(goodGuys);
                fight.BadGuys = JsonConvert.SerializeObject(badGuys);
                fight.FightDetails = JsonConvert.SerializeObject(fightDetails);

                DataService.CreateFight(fight);

                fightvm.FightId = fightId;
                fightvm.GoodGuys = goodGuys;
                fightvm.BadGuys = badGuys;
                fightvm.FightDetails = fightDetails;
            }
            catch (Exception ex)
            {
                UlockCharactersFromFight(partyCharacters);
                throw new Exception(message: ex.Message);
            }

            return fightvm;
        }
        #endregion

        #region PublicGetters
        public string GetFightById(RequestVm request)
        {
            var charvm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            var chr = ValidateCharacterByPlayerId(charvm.CharacterId, charvm.PlayerId);

            var fight = GetFightById(chr.FightId.GetValueOrDefault());

            if (fight == null)
            {
                throw new Exception(string.Join(": ", Scribe.ShortMessages.BadRequest, "no fight found for supplied character id."));
            }

            var fightvm = new FightVm(fight);

            return JsonConvert.SerializeObject(fightvm);
        }
        #endregion

    }
}
