using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models
{
    public partial class Fight
    {
        public Guid Id { get; set; }

        public string GoodGuys { get; set; }
        public string BadGuys { get; set; }

        public string FightDetails { get; set; }
    }
}
