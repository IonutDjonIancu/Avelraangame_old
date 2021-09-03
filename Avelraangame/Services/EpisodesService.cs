using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using System;

namespace Avelraangame.Services
{
    public class EpisodesService : EpisodesSubService
    {
        #region BusinessLogic
        public string CreateEpisode(RequestVm request)
        {
            var epiVm = ValidateRequestDeserializationIntoEpisodeVm(request.Message);

            ValidateSigma(epiVm.SigmaWard);

            var episode = new Episode 
            {
                Id = Guid.NewGuid(),
                Name = epiVm.Name,
                Date = epiVm.Date,
                Prologue = epiVm.Prologue,
                Epilogue = epiVm.Epilogue
            };

            ValidateEpisodeExists(episode.Name);

            DataService.CreateEpisode(episode);

            return string.Concat(Scribe.ShortMessages.Success, ": episode created.");
        }

        #endregion

        #region PublicGetters
        public EpisodeVm GetEpisodeByName(string episodeName)
        {
            ValidateEpisodeName(episodeName);

            return ConvertEpisodeToVm(DataService.GetEpisodeByName(episodeName));
        }
        #endregion


    }
}
