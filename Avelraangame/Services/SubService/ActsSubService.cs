using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.SubService
{
    public class ActsSubService : ServiceBase
    {
        protected string CreateAct(ActVm actVm)
        {
            var episodeService = new EpisodesService();

            ValidateActExists(actVm.Name);

            var episode = episodeService.GetEpisodeByName(actVm.EpisodeName);

            if (episode == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": episode name is missing or invalid."));
            }

            var act = new Act
            {
                Id = Guid.NewGuid(),
                Name = actVm.Name,
                EpisodeId = episode.Id,
                Difficulty = actVm.Difficulty
            };

            DataService.CreateAct(act);

            return string.Concat(Scribe.ShortMessages.Success, $": act added to episode: {episode.Name}.");
        }

        protected string UpdateAct(ActVm actVm)
        {
            ValidateActName(actVm.Name);

            var episodeService = new EpisodesService();
            var episode = episodeService.GetEpisodeByName(actVm.EpisodeName);

            if (episode == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": episode name is missing or invalid."));
            }

            var act = DataService.GetActByName(actVm.Name);

            if (actVm.Difficulty != null)
            {
                act.Difficulty = actVm.Difficulty;
            }
            if (actVm.EpisodeName != null)
            {
                act.EpisodeId = episode.Id;
            }

            DataService.UpdateAct(act);

            return string.Concat(Scribe.ShortMessages.Success, $": act updated to episode: {episode.Name}.");
        }

        protected string DeleteAct(ActVm actVm)
        {
            ValidateActName(actVm.Name);

            var act = DataService.GetActByName(actVm.Name);

            if (act == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": act not found."));
            }

            DataService.DeleteAct(act);

            return string.Concat(Scribe.ShortMessages.Success, ": act deleted.");
        }
    }
}
