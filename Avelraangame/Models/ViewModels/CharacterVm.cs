using Avelraangame.Models.ModelScraps;
using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;

namespace Avelraangame.Models.ViewModels
{
    public class CharacterVm
    {
        public Guid CharacterId { get; set; }
        public Guid? PlayerId { get; set; }
        public string PlayerName { get; set; }

        public string Name { get; set; }
        public string Race { get; set; }
        public string Culture { get; set; }

        public int Strength { get; set; }
        public int Toughness { get; set; }
        public int Awareness { get; set; }
        public int Abstract { get; set; }

        public int EntityLevel { get; set; }
        public int Experience { get; set; }
        public int DRM { get; set; }
        public int Wealth { get; set; }

        public int Harm { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }

        public bool IsAlive { get; set; }
        public bool InParty { get; set; }
        public Guid? PartyId { get; set; }


        public Equippment Equippment { get; set; }
        public List<HeroicTraits> HeroicTraits { get; set; }
        public List<NegativePerks> NegativePerks { get; set; }


        public Logbook Logbook { get; set; }
        public List<Item> Supplies { get; set; }

        public CharacterVm()
        {
            Equippment = new Equippment();
            HeroicTraits = new List<HeroicTraits>();
            NegativePerks = new List<NegativePerks>();
            Logbook = new Logbook();
        }
    }
}
