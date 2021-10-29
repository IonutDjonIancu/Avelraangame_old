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

        public FightVm()
        {

        }

        public FightVm(Fight fight)
        {
            FightId = fight.Id;
            GoodGuys = JsonConvert.DeserializeObject<List<CharacterVm>>(fight.GoodGuys);
            BadGuys = JsonConvert.DeserializeObject<List<CharacterVm>>(fight.BadGuys);
            FightDetails = JsonConvert.DeserializeObject<FightDetails>(fight.FightDetails);
        }
    }
}
