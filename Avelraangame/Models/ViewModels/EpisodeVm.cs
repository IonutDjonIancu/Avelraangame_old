using System;

namespace Avelraangame.Models.ViewModels
{
    public class EpisodeVm
    {
        public Guid EpisodeId { get; set; }

        public string Name { get; set; }

        public int Date { get; set; }

        public string Prologue { get; set; }
        public string Epilogue { get; set; }
    }
}
