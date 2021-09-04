using System;

namespace Avelraangame.Models.ViewModels
{
    public class ActVm
    {
        public Guid ActId { get; set; }

        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string EpisodeName { get; set; }

        public string ActCrudOperation { get; set; }
        public string SigmaWard { get; set; }
    }
}
