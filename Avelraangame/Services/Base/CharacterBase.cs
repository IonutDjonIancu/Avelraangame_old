using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;

namespace Avelraangame.Services.Base
{
    public class CharacterBase
    {
        protected DataService DataService { get; set; }

        protected CharacterBase()
        {
            DataService = new DataService();
        }

        protected CharacterBehalfVm ValidateRequestDeserialization_CharacterBehalfVm(string request)
        {
            CharacterBehalfVm charBehalfVm;

            try
            {
                charBehalfVm = JsonConvert.DeserializeObject<CharacterBehalfVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return charBehalfVm;
        }
    }
}
