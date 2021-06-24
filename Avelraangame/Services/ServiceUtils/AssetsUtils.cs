using Avelraangame.Models.ModelScraps;
using Avelraangame.Models.ViewModels;
using System.Collections.Generic;

namespace Avelraangame.Services.ServiceUtils
{
    public static class AssetsUtils
    {
        public static List<string> ListOfAssets = new List<string>()
        {
            Scribe.Assets_Health, // 0
            Scribe.Assets_Mana, // 1
            Scribe.Assets_Harm, // 2
            Scribe.Assets_DRM, // 3
            Scribe.Assets_Experience, // 4
            Scribe.Assets_Wealth // 5
        };

        //public static int CalculateHealth(int tou, int str, int awa, int exp, int type, StatsVm baseAndBonus)
        //{
            



        //}
    }
}
