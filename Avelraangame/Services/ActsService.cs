using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services
{
    public class ActsService : ActsSubService
    {
        #region BusinessLegion
        #endregion

        #region PublicGetters
        public string GetDifficultyLevels()
        {
            var list = new List<string>
            {
                Scribe.Difficulty.D1_Easy,
                Scribe.Difficulty.D2_Normal,
                Scribe.Difficulty.D3_Hard,
                Scribe.Difficulty.D4_Heroic,
                Scribe.Difficulty.D5_Legendary,
                Scribe.Difficulty.D6_Astral
            };

            return JsonConvert.SerializeObject(list);
        }
        #endregion
    }
}
