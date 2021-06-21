using Avelraangame.Models.ModelScraps;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.SubService
{
    public class SkillsSubService
    {
        public ItemBonuses ReturnRandomSkillIncreaseForItem(ItemBonuses bonuses, int increasedBy)
        {
            var randomSkill = Dice.Roll_min_to_max(0, 15);

            switch (randomSkill)
            {
                case 0:
                    bonuses.ToApothecary += increasedBy;
                    return bonuses;
                case 1:
                    bonuses.ToArcane += increasedBy;
                    return bonuses;
                case 2:
                    bonuses.ToDodge += increasedBy;
                    return bonuses;
                case 3:
                    bonuses.ToHide += increasedBy;
                    return bonuses;
                case 4:
                    bonuses.ToMelee += increasedBy;
                    return bonuses;
                case 5:
                    bonuses.ToNavigation += increasedBy;
                    return bonuses;
                case 6:
                    bonuses.ToPsionics += increasedBy;
                    return bonuses;
                case 7:
                    bonuses.ToRanged += increasedBy;
                    return bonuses;
                case 8:
                    bonuses.ToResistance += increasedBy;
                    return bonuses;
                case 9:
                    bonuses.ToScouting += increasedBy;
                    return bonuses;
                case 10:
                    bonuses.ToSocial += increasedBy;
                    return bonuses;
                case 11:
                    bonuses.ToSpot += increasedBy;
                    return bonuses;
                case 12:
                    bonuses.ToSurvival += increasedBy;
                    return bonuses;
                case 13:
                    bonuses.ToTactics += increasedBy;
                    return bonuses;
                case 14:
                    bonuses.ToTraps += increasedBy;
                    return bonuses;
                case 15:
                    bonuses.ToUnarmed += increasedBy;
                    return bonuses;
                default:
                    throw new Exception(message: Scribe.Error_Switch_notCoveredCase);
            }
        }
    }
}
