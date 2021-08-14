using Avelraangame.Models.ModelScraps;
using System;

namespace Avelraangame.Models.ViewModels
{
    public class CombatVm
    {
        public Guid Id { get; set; }

        public Fight Combatants { get; set; }

        public Guid AttackerId { get; set; }
        public Guid DefenderId { get; set; }
    }
}
