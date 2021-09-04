using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.SubService
{
    public class EpisodesSubService : ServiceBase
    {
        protected EpisodeVm ConvertEpisodeToVm(Episode episode)
        {
            var epVm = new EpisodeVm
            {
                EpisodeId = episode.Id,
                Name = episode.Name,
                Story = episode.Story,
                Date = episode.Date,
                Prologue = episode.Prologue,
                Epilogue = episode.Epilogue
            };

            return epVm;
        }

        protected string CreateEpisode(EpisodeVm epiVm)
        {
            ValidateEpisodeExists(epiVm.Name);
            
            var episode = new Episode
            {
                Id = Guid.NewGuid(),
                Name = epiVm.Name,
                Story = epiVm.Story,
                Date = epiVm.Date,
                Prologue = epiVm.Prologue,
                Epilogue = epiVm.Epilogue
            };

            DataService.CreateEpisode(episode);

            return string.Concat(Scribe.ShortMessages.Success, ": episode created.");
        }

        protected string UpdateEpisode(EpisodeVm epiVm)
        {
            ValidateEpisodeName(epiVm.Name);

            var episode = DataService.GetEpisodeByName(epiVm.Name);

            if (episode == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": episode not found."));
            }

            if (epiVm.Story != null)
            {
                episode.Story = epiVm.Story;
            }
            if (epiVm.Date != 0)
            {
                episode.Date = epiVm.Date;
            }
            if (epiVm.Prologue != null)
            {
                episode.Prologue = epiVm.Prologue;
            }
            if (epiVm.Epilogue != null)
            {
                episode.Epilogue = epiVm.Epilogue;
            }

            DataService.UpdateEpisode(episode);

            return string.Concat(Scribe.ShortMessages.Success, ": episode updated.");
        }

        protected string DeleteEpisode(EpisodeVm epiVm)
        {
            ValidateEpisodeExists(epiVm.Name);

            var episode = DataService.GetEpisodeByName(epiVm.Name);

            if (episode == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": episode not found."));
            }

            DataService.DeleteEpisode(episode);

            return string.Concat(Scribe.ShortMessages.Success, ": episode deleted.");
        }
    }
}
