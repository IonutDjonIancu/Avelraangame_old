using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.SubService
{
    public class CombatSubService : ServiceBase
    {
        protected (CharacterVm att, CharacterVm def) RollAttack(CharacterVm attacker, CharacterVm defender)
        {
            var attackerRoll = Dice.Roll_d_20();
            var attackResult = attacker.Skills.Melee * attackerRoll / 10;
            var defenderRoll = Dice.Roll_d_20();
            var defendResult = defender.Skills.Melee * defenderRoll / 10;

            var rollResult = attackResult - defendResult;
            if (rollResult <= 0)
            {
                return (attacker, defender);
            }

            var damage = rollResult - rollResult * defender.Expertise.DRM / 100;
            defender.Assets.Health -= damage;

            return (attacker, defender);
        }
    }
}
