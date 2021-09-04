using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;

namespace Avelraangame.Models.POCOs
{
    public class Fight
    {
        public Guid FightId { get; set; }
        public List<CharacterVm> GoodGuys { get; set; }
        public List<CharacterVm> BadGuys { get; set; }

        public CombatUtils.TacticalSituation TacticalSituation { get; set; }

        public string LastActionResult { get; set; }
        public string FightDate { get; set; }
    }
}
