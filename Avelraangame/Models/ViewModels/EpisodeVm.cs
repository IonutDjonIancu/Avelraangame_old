using System;
using System.Collections.Generic;

namespace Avelraangame.Models.ViewModels
{
    public class EpisodeVm
    {
        public Guid EpisodeId { get; set; }


        public string Name { get; set; }
        public string Story { get; set; }
        public int Date { get; set; }
        public string Prologue { get; set; }
        public string Epilogue { get; set; }


        public string EpisodeCrudOperation { get; set; }
        public string SigmaWard { get; set; }

        public List<Act> Acts { get; set; }
    }
}
