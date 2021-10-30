using System.Collections.Generic;

namespace Avelraangame.Services.ServiceUtils
{
    public static class PlayersUtils
    {
        public static class PlayerSymbols
        {
            public static string Epsilon = "Epsilon";
            public static string Omicron = "Omicron";
            public static string Sigma = "Sigma";
        }

        public static List<string> PlayerSymbolsList = new List<string>
        {
            PlayerSymbols.Epsilon,
            PlayerSymbols.Omicron,
            PlayerSymbols.Sigma
        };

    }
}
