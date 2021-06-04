using System.Collections.Generic;

namespace Avelraangame.Services.ServiceUtils
{
    public static class StatsUtils
    {
        public static List<string> ListOfStats = new List<string>()
        {
            Scribe.Stats_Strength, // 0
            Scribe.Stats_Toughness, // 1
            Scribe.Stats_Awareness, // 2
            Scribe.Stats_Abstract // 3
        };
    }
}
