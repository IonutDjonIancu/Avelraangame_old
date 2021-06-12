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
        private PlayersService PlayersService { get; set; }

        public CharactersService()
        {
            PlayersService = new PlayersService();
        }

        public (string responseMessage, string playerId, int roll) CharacterRoll20(RequestVm request)
        {
            CharacterBehalfVm charBehalfVm;

            try
            {
                charBehalfVm = ValidateRequestDeserialization_CharacterBehalfVm(request.Message);
            }
            catch (Exception ex)
            {
                return (string.Concat(Scribe.ShortMessages.Failure, ": ", ex.Message), null, 0);
            }

            var playerId = PlayersService.GetPlayerByName(charBehalfVm.PlayerName).Id;

            var roll = Dice.Roll_d_20();
            
            var characterDiceVm = new CharacterDiceVm
            {
                PlayerId = playerId,
                DiceRoll = roll
            };


            //var storage = KeyValuePair.Create<string, object>(playerId.ToString(), roll);


            //if (!TempData.ContainsKey(playerId.ToString()))
            //{
            //    TempData.Add(storage);
            //}

            return (JsonConvert.SerializeObject(characterDiceVm), playerId.ToString(), roll);
        }

        //public string CreateCharacter(RequestVm request)
        //{
        //    CharacterVm characterVm;

        //    var a = KeyValuePair.Create<string, object>("aaa", 22);
        //    var b = KeyValuePair.Create<string, object>("bbb", 33);

        //    if (!TempData.ContainsKey("aaa"))
        //    {
        //        TempData.Add(a);
        //    }
        //    if (!TempData.ContainsKey("bbb"))
        //    {
        //        TempData.Add(b);
        //    }


        //    var c = TempData.Peek("aaa");

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //    }





        //    return "";
        //}


    }
}
