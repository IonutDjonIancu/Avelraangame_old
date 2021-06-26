using Avelraangame.Models.ModelScraps;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
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
        public List<ItemVm> Supplies { get; set; }
        public List<TempsVm> Bonuses { get; set; }

        public CharacterVm()
        {
            Equippment = new Equippment();
            HeroicTraits = new List<HeroicTraits>();
            NegativePerks = new List<NegativePerks>();
            Logbook = new Logbook();
        }

        public CharacterVm(Character chr)
        {
            CharacterId = chr.Id;
            PlayerId = chr.PlayerId;
            PlayerName = chr.Player?.Name;

            Name = chr.Name;
            Race = ((CharactersUtils.Races)chr.Race).ToString();
            Culture = ((CharactersUtils.Cultures)(chr.Culture)).ToString();

            Strength = chr.Strength;
            Toughness = chr.Toughness;
            Awareness = chr.Awareness;
            Abstract = chr.Abstract;

            EntityLevel = chr.EntityLevel;
            Experience = chr.Experience;
            DRM = chr.DRM;
            Wealth = chr.Wealth;

            Harm = chr.Harm;
            Health = chr.Health;
            Mana = chr.Mana;

            IsAlive = chr.IsAlive;
            InParty = chr.InParty;
            PartyId = chr.PartyId;

            if (chr.Equippment != null)
            {
                Equippment = JsonConvert.DeserializeObject<Equippment>(chr.Equippment);
            }

            if (chr.HeroicTraits != null)
            {
                HeroicTraits = JsonConvert.DeserializeObject<List<HeroicTraits>>(chr.HeroicTraits);
            }

            if (chr.NegativePerks != null)
            {
                NegativePerks = JsonConvert.DeserializeObject<List<NegativePerks>>(chr.NegativePerks);
            }

            if (chr.Logbook != null)
            {
                Logbook = JsonConvert.DeserializeObject<Logbook>(chr.Logbook);
            }

            if (chr.Supplies != null)
            {
                Supplies = JsonConvert.DeserializeObject<List<ItemVm>>(chr.Supplies);
            }

        }
    }
}
