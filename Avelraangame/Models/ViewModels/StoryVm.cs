using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.ViewModels
{
    public class StoryVm
    {
        public Guid CharacterId { get; set; }
        public Guid PlayerId { get; set; }
        public Guid ActId { get; set; }
    }
}
