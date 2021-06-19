using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.SubService
{
    public class TempsSubService : TempsBase // does not require a a main service
    {
        public void SaveTempPlayerData(string keyPlayerInfo, string values)
        {
            DataService.SaveTempPlayerData(keyPlayerInfo, values);
        }

        public string GetTempPlayerData(string playerUnicity)
        {
            return DataService.GetTempPlayerData(playerUnicity);
        }

        public void DeleteTempPlayerData(string playerUnicity)
        {
            DataService.DeleteTempPlayerData(playerUnicity);
        }
    }
}
