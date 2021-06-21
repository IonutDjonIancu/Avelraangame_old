using Avelraangame.Models.ModelScraps;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.SubService
{
    public class AssetsSubService
    {
        public ItemBonuses ReturnRandomAssetIncreaseForItem(ItemBonuses bonuses, int increasedBy)
        {
            var randomSkill = Dice.Roll_min_to_max(0, 5);

            switch (randomSkill)
            {
                case 0:
                    bonuses.ToHealth += increasedBy;
                    return bonuses;
                case 1:
                    bonuses.ToMana += increasedBy;
                    return bonuses;
                case 2:
                    bonuses.ToHarm += increasedBy;
                    return bonuses;
                case 3:
                    bonuses.ToDRM += increasedBy;
                    return bonuses;
                case 4:
                    bonuses.ToExperience += increasedBy;
                    return bonuses;
                case 5:
                    bonuses.ToWealth += increasedBy;
                    return bonuses;
                default:
                    throw new Exception(message: Scribe.Error_Switch_notCoveredCase);
            }
        }
    }
}
