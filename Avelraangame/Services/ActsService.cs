using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services
{
    public class ActsService : ActsSubService
    {
        #region BusinessLegion
        public string ActCRUD(RequestVm request)
        {
            var actVm = ValidateRequestDeserializationIntoActVm(request.Message);

            ValidateSigma(actVm.SigmaWard);

            if (actVm.ActCrudOperation.Equals(Scribe.CrudActions.Create))
            {
                return CreateAct(actVm);
            }
            else if (actVm.ActCrudOperation.Equals(Scribe.CrudActions.Read))
            {
                return ""; // TODO: decide what to do when a read action is requested
            }
            else if (actVm.ActCrudOperation.Equals(Scribe.CrudActions.Update))
            {
                return UpdateAct(actVm);
            }
            else if (actVm.ActCrudOperation.Equals(Scribe.CrudActions.Delete))
            {
                return DeleteAct(actVm);
            }

            return string.Concat(Scribe.ShortMessages.Failure, ": no CRUD action was selected.");
        }
        #endregion

        #region PublicGetters
        public string GetDifficultyLevels()
        {
            var list = new List<string>
            {
                Scribe.Difficulty.D0_Custom,
                Scribe.Difficulty.D1_Easy,
                Scribe.Difficulty.D2_Normal,
                Scribe.Difficulty.D3_Hard,
                Scribe.Difficulty.D4_Heroic,
                Scribe.Difficulty.D5_Legendary,
                Scribe.Difficulty.D6_Astral
            };

            return JsonConvert.SerializeObject(list);
        }
        #endregion
    }
}
