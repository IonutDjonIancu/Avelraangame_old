using System;

namespace Avelraangame.Models.ViewModels
{
    public class AttackVm
    {
        public Guid FightId { get; set; }

        public Guid PlayerId { get; set; }

        public Guid AttackerId { get; set; }

        public Guid TargetId { get; set; }
    }
}
