using Avelraangame.Models;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services
{
    public class TempsService : TempsSubService
    {
        public void SaveTempCharInfo(Guid charId, TempUtils.BonusTo bonusTo, int value, string description = null)
        {
            var temps = new TempInfo
            {
                Id = Guid.NewGuid(),
                CharacterId = charId,
                BonusTo = bonusTo,
                Value = value,
                Description = description,
            };

            ValidateTempInfo(temps);

            DataService.SaveTempCharacterInfo(temps);
        }


        public List<TempInfo> GetTempInfosByCharacterId(Guid charId)
        {
            return DataService.GetTempInfosByCharacterId(charId);
        }




    }
}
