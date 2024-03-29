﻿using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;

namespace Avelraangame.Models.ViewModels
{
    public class FightVmOld
    {
        public Guid FightId { get; set; }
        public List<CharacterVm> GoodGuys { get; set; }
        public List<CharacterVm> BadGuys { get; set; }

        public CombatUtils.TacticalSituation TacticalSituation { get; set; }

        public string LastActionResult { get; set; }
        public string FightDate { get; set; }
    }
}
