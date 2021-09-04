using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;

namespace Avelraangame.Services
{
    public class EpisodesService : EpisodesSubService
    {
        #region BusinessLogic
        public string EpisodeCRUD(RequestVm request)
        {
            var epiVm = ValidateRequestDeserializationIntoEpisodeVm(request.Message);

            ValidateSigma(epiVm.SigmaWard);

            if (epiVm.EpisodeCrudAction.Equals(Scribe.CrudActions.Create))
            {
                return CreateEpisode(epiVm);
            }
            else if (epiVm.EpisodeCrudAction.Equals(Scribe.CrudActions.Read))
            {
                return ""; // TODO: decide what to do when a read action is requested
            }
            else if (epiVm.EpisodeCrudAction.Equals(Scribe.CrudActions.Update))
            {
                return UpdateEpisode(epiVm);
            }
            else if (epiVm.EpisodeCrudAction.Equals(Scribe.CrudActions.Delete))
            {
                return DeleteEpisode(epiVm);
            }
            
            return string.Concat(Scribe.ShortMessages.Failure, ": no CRUD action was selected.");
        }
        #endregion

        #region PublicGetters
        public Episode GetEpisodeByName(string episodeName)
        {
            ValidateEpisodeName(episodeName);

            return DataService.GetEpisodeByName(episodeName);
        }

        public string GetEpisodes()
        {
            return JsonConvert.SerializeObject(DataService.GetEpisodes());
        }
        #endregion


    }
}
