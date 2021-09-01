using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Models
{
    public class Act
    {
        // questlines are culture specific

        public Guid Id { get; set; }
        public ActsUtils.Stories Story { get; set; }
        public string Name { get; set; }

        public ActsUtils.ActDifficulty Difficulty { get; set; }

        public Guid? EpisodeId { get; set; }
        public Episode Episode { get; set; }
    }
}
