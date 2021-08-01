using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services
{
    public class CombatService : CombatSubService
    {
        #region Business logic
        

        #endregion

        #region Getters
        public string GetFightIdByCharacterId(RequestVm request)
        {
            var charVm = ValidateRequestDeserializationIntoCharacterVm(request.Message);
            var character = ValidateCharacterById(charVm.CharacterId);

            if (character.InFight.GetValueOrDefault())
            {
                return character.FightId.ToString();
            }

            return string.Concat(Scribe.ShortMessages.ResourceNotFound, ": character is not fighting.");
        }
        #endregion


    }
}
