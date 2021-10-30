using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models
{
    public class Episode
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Story { get; set; } // TODO: moved to a separate table

        public int Date { get; set; }

        public string Prologue { get; set; }

        public string Epilogue { get; set; }

        public ICollection<Act> Acts { get; set; }
    }
}
