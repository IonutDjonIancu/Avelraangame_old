using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.ServiceUtils
{
    public class CombatUtils
    {
        public enum TacticalSituation
        {
            Major_tactical_advantage = 1,
            Slight_tactical_advantage,
            OnPar,
            Slight_tactical_disadvantage,
            Major_tactical_disadvantage
        }
    }
}
