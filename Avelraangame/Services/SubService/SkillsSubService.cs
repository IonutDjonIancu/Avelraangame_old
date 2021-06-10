using Avelraangame.Models.ModelProps;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.SubService
{
    public class SkillsSubService
    {
        public ItemProperties ReturnRandomSkillIncreaseForItem(ItemProperties props, int increasedBy)
        {
            var randomSkill = Dice.Roll_min_to_max(0, 15);

            switch (randomSkill)
            {
                case 0:
                    props.Skills_toApothecary += increasedBy;
                    return props;
                case 1:
                    props.Skills_toArcane += increasedBy;
                    return props;
                case 2:
                    props.Skills_toDodge += increasedBy;
                    return props;
                case 3:
                    props.Skills_toHide += increasedBy;
                    return props;
                case 4:
                    props.Skills_toMelee += increasedBy;
                    return props;
                case 5:
                    props.Skills_toNavigation += increasedBy;
                    return props;
                case 6:
                    props.Skills_toPsionics += increasedBy;
                    return props;
                case 7:
                    props.Skills_toRanged += increasedBy;
                    return props;
                case 8:
                    props.Skills_toResistance += increasedBy;
                    return props;
                case 9:
                    props.Skills_toScouting += increasedBy;
                    return props;
                case 10:
                    props.Skills_toSocial += increasedBy;
                    return props;
                case 11:
                    props.Skills_toSpot += increasedBy;
                    return props;
                case 12:
                    props.Skills_toSurvival += increasedBy;
                    return props;
                case 13:
                    props.Skills_toTactics += increasedBy;
                    return props;
                case 14:
                    props.Skills_toTraps += increasedBy;
                    return props;
                case 15:
                    props.Skills_toUnarmed += increasedBy;
                    return props;
                default:
                    throw new Exception(message: Scribe.Error_Switch_notCoveredCase);
            }
        }
    }
}
