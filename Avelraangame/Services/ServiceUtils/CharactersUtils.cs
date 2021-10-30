using Avelraangame.Models.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.ServiceUtils
{
    public static class CharactersUtils
    {
        public enum Races
        {
            Human = 1,
            Elf,
            Dwarf,
            // TODO: add the rest of the races
        }

        public enum Cultures
        {
            Danarian = 1,
            Ravanon,
            Midheim,
            Endarian,
            // TOD: add the rest of the cultures
        }
    }
}
