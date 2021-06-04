using Avelraangame.Models.ModelProps;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.ServiceBase
{
    public class StatsServiceBase
    {
        public ItemProperties ReturnRandomStatIncreaseForItem(ItemProperties props, int increasedBy)
        {
            var randomSkill = Dice.Roll_min_to_max(0, 3);

            switch (randomSkill)
            {
                case 0:
                    props.Stats_toStrength += increasedBy;
                    return props;
                case 1:
                    props.Stats_toToughness += increasedBy;
                    return props;
                case 2:
                    props.Stats_toAwareness += increasedBy;
                    return props;
                case 3:
                    props.Stats_toAbstract += increasedBy;
                    return props;
                default:
                    throw new Exception(message: Scribe.Error_Switch_notCoveredCase);
            }
        }
    }
}
