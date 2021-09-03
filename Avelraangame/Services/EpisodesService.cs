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

        public string UpdateEpisode(RequestVm request)
        {
            var epiVm = ValidateRequestDeserializationIntoEpisodeVm(request.Message);

            ValidateSigma(epiVm.SigmaWard);

            var episode = GetEpisodeByName(epiVm.Name);

            if (episode == null)
            {
                throw new Exception(string.Concat(Scribe.ShortMessages.ResourceNotFound, ": episode with this name not found."));
            }

            episode.Date = epiVm.Date;
            episode.Prologue = epiVm.Prologue;
            episode.Epilogue = epiVm.Epilogue;

            ValidateEpisodeExists(episode.Name);

            DataService.UpdateEpisode(episode);

            return string.Concat(Scribe.ShortMessages.Success, ": episode created.");
        }

        #endregion

        #region PublicGetters
        public Episode GetEpisodeByName(string episodeName)
        {
            ValidateEpisodeName(episodeName);

            return DataService.GetEpisodeByName(episodeName);
        }
        #endregion


    }
}
