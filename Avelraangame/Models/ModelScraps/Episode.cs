using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.ModelScraps
{
    public class Episode
    {
        public string Title { get; set; }

        public int Year { get; set; }

        public string Prologue { get; set; }

        public List<Act> Acts { get; set; }

        public string Epilogue { get; set; }
    }
}
