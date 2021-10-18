using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;

namespace Avelraangame.Models.POCOs
{
    public class FightDetails
    {
        public CombatUtils.TacticalSituation TacticalSituation { get; set; }
        public string Difficulty { get; set; }
        public string LastActionResult { get; set; }
        public DateTime StartDate { get; set; }
        public List<ItemVm> Loot { get; set; }
        public string Renown { get; set; }
    }
}
