using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.ModelScraps
{
    public class Attack
    {
        public Guid FightId { get; set; }

        public Guid Attacker { get; set; }

        public Guid Defender { get; set; }


    }
}
