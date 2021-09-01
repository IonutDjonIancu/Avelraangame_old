using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models
{
    public class Quest
    {

        // questlines are culture related

        public Guid Id { get; set; }
        public string Name { get; set; }

        public string EpisodeDetails { get; set; }

    }
}
