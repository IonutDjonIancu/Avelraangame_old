using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.POCOs
{
    public class Logbook
    {
        public int Wealth { get; set; }
        public int EntityLevel { get; set; }
        public int StatsRoll { get; set; }
        public int ItemsRoll { get; set; }
        public int PortraitNr { get; set; }
        public string Race { get; set; }
        public string Culture { get; set; }
        public int Fights { get; set; }
        public List<string> Renown { get; set; }

        //public LocationUtils.Location Location { get; set; }

        //public string Story { get; set; }
    }
}
