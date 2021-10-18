using Avelraangame.Models.POCOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Avelraangame.Models.ViewModels
{
    public class FightVm
    {
        public Guid FightId { get; set; }

        public List<CharacterVm> GoodGuys { get; set; }
        public List<CharacterVm> BadGuys { get; set; }

        public FightDetails FightDetails { get; set; }
    }
}
