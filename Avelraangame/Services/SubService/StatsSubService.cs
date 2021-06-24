using Avelraangame.Models.ModelScraps;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.SubService
{
    public class StatsSubService
    {
        public Bonuses ReturnRandomStatIncreaseForItem(Bonuses bonuses, int increasedBy)
        {
            var randomSkill = Dice.Roll_min_to_max(0, 3);

            switch (randomSkill)
            {
                case 0:
                    bonuses.ToStrength += increasedBy;
                    return bonuses;
                case 1:
                    bonuses.ToToughness += increasedBy;
                    return bonuses;
                case 2:
                    bonuses.ToAwareness += increasedBy;
                    return bonuses;
                case 3:
                    bonuses.ToAbstract += increasedBy;
                    return bonuses;
                default:
                    throw new Exception(message: Scribe.Error_Switch_notCoveredCase);
            }
        }
    }
}
