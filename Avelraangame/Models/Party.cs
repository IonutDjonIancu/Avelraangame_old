using System;
using System.Collections.Generic;

namespace Avelraangame.Models
{
    public partial class Party
    {
        public Guid Id { get; set; }

        public bool IsPartyLocked { get; set; }

        public ICollection<Character> Characters{ get; set; }
    }
}
