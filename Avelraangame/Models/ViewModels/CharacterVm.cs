using Avelraangame.Models.ModelScraps;
using System;

namespace Avelraangame.Models.ViewModels
{
    public class CharacterVm
    {
        public Guid CharacterId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int StatsRoll { get; set; }

        public StatsVm Stats { get; set; }
        public ExpertiseVm Expertise { get; set; }
        public AssetsVm Assets { get; set; }

        public Logbook Logbook { get; set; }
    }
}
