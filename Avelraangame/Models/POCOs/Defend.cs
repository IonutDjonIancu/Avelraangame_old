using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.POCOs
{
    public class Defend
    {
        public Guid FightId { get; set; }

        public Guid MainCharacterId { get; set; }

        public Guid PlayerId { get; set; }
    }
}
