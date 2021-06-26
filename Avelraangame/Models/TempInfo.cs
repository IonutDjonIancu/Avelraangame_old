using Avelraangame.Services.ServiceUtils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Avelraangame.Models
{
    public partial class TempInfo
    {
        public Guid Id { get; set; }
        public Guid CharacterId { get; set; }


        public TempUtils.BonusTo BonusTo { get; set; }

        public int Value { get; set; }
        public string Description { get; set; }

        public Character Character { get; set; }
    }
}
