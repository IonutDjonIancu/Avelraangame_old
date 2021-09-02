using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;

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
                Date = episode.Date,
                Prologue = episode.Prologue,
                Epilogue = episode.Epilogue
            };

            return epVm;
        }
    }
}
