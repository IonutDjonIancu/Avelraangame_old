using System;
using System.Collections.Generic;

namespace Avelraangame.Models
{
    public partial class Character
    {
        public Guid Id { get; set; }
        public Guid? PlayerId { get; set; }

        public string Name { get; set; }

        public string Stats { get; set; }
        public string Assets { get; set; }
        public string Expertise { get; set; }
        public string Skills { get; set; }

        public bool IsAlive { get; set; }
        public bool HasLevelup { get; set; }
        public bool InParty { get; set; }
        public Guid? PartyId { get; set; }

        public string Equippment { get; set; }
        public string HeroicTraits { get; set; }
        public string NegativePerks { get; set; }

        public string Logbook { get; set; }
        public string Supplies { get; set; }

        public Player Player { get; set; }
        public Party Party { get; set; }

        public ICollection<TempInfo> Temps { get; set; }
    }
}
