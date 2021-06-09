using Avelraangame.Models.ModelProps;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.SubService
{
    public class AssetsSubService
    {
        public ItemProperties ReturnRandomAssetIncreaseForItem(ItemProperties props, int increasedBy)
        {
            var randomSkill = Dice.Roll_min_to_max(0, 5);

            switch (randomSkill)
            {
                case 0:
                    props.Assets_toHealth += increasedBy;
                    return props;
                case 1:
                    props.Assets_toMana += increasedBy;
                    return props;
                case 2:
                    props.Assets_toHarm += increasedBy;
                    return props;
                case 3:
                    props.Assets_toDRM += increasedBy;
                    return props;
                case 4:
                    props.Assets_toExperience += increasedBy;
                    return props;
                case 5:
                    props.Assets_toWealth += increasedBy;
                    return props;
                default:
                    throw new Exception(message: Scribe.Error_Switch_notCoveredCase);
            }
        }
    }
}
