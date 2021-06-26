using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.ViewModels
{
    public class TempsVm
    {
        public Guid CharacterId { get; set; }
        public string BonusTo { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }

        public TempsVm(TempInfo temps)
        {
            CharacterId = temps.CharacterId;
            BonusTo = ((TempUtils.BonusTo)temps.BonusTo).ToString();
            Value = temps.Value;
            Description = temps.Description;
        }
    }
}
