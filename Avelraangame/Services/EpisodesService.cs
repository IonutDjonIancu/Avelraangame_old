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
            var episode = new Episode // currently hardcoded, must be changed to use the API request
            {
                Id = Guid.NewGuid(),
                Name = "Blade for hire",
                Date = 0,
                Prologue = "The sword made crowns, the crowns made swords.",
                Epilogue = "The world turns, the days pass"
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
