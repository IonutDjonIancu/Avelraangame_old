using Avelraangame.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.ModelScraps
{
    public class Combat
    {
        public Guid FightId { get; set; }
        public List<CharacterVm> GoodGuys { get; set; }
        public List<CharacterVm> BadGuys { get; set; }

        public string FightDate { get; set; }


    }
}
