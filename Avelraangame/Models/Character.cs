using System;
using System.Collections.Generic;

namespace Avelraangame.Models
{
    public partial class Character
    {
        public Guid Id { get; set; }
        public Guid? PlayerId { get; set; }

        public string Name { get; set; }
        public int Race { get; set; }
        public int Culture { get; set; }

        public int Strength { get; set; }
        public int Toughness { get; set; }
        public int Awareness { get; set; }
        public int Abstract { get; set; }

        public int EntityLevel { get; set; }
        public int Experience { get; set; }
        public int DRM { get; set; }
        public int Wealth { get; set; }

        public int Health { get; set; }
        public int Mana { get; set; }
        public int Harm { get; set; }

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
