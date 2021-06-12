using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Avelraangame.Services
{
    public class CharactersService : CharacterSubService
    {
        public CharactersService()
        {
        }

        public (string responseMessage, string playerId, int roll) CharacterRoll20(RequestVm request)
        {
            CharacterBehalfVm charBehalfVm;
            Guid playerId;

            try
            {
                charBehalfVm = ValidateRequestDeserialization_CharacterBehalfVm(request.Message);
                
                if (charBehalfVm.PlayerId == null || charBehalfVm.PlayerId.Equals(Guid.Empty))
                {
                    playerId = ValidatePlayer(charBehalfVm.PlayerName).Id;
                }
                else
                {
                    playerId = charBehalfVm.PlayerId;
                }
            }
            catch (Exception ex)
            {
                return (string.Concat(Scribe.ShortMessages.Failure, ": ", ex.Message), null, 0);
            }

            var roll = Dice.Roll_d_20();
            
            var characterDiceVm = new CharacterDiceVm
            {
                PlayerId = playerId,
                DiceRoll = roll
            };

            return (JsonConvert.SerializeObject(characterDiceVm), playerId.ToString(), roll);
        }







    }
}
