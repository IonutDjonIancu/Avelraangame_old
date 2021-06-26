using Avelraangame.Models.ModelScraps;
using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;

namespace Avelraangame.Models.ViewModels
{
    public class CharacterCalculatedVm
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


        public CharacterCalculatedVm(CharacterVm charVm)
        {
            CharacterId = charVm.CharacterId;
            PlayerId = charVm.PlayerId;

            Name = charVm.Name;
            Race = charVm.Race;
            Culture = charVm.Culture;

            Wealth = charVm.Wealth;
            EntityLevel = charVm.EntityLevel;

            IsAlive = charVm.IsAlive;
            InParty = charVm.InParty;
            PartyId = charVm.PartyId;

            // stats
            Strength = CharactersUtils.CalculateStrength(charVm.Strength, charVm.Equippment);
            Toughness = CharactersUtils.CalculateToughness(charVm.Toughness, charVm.Equippment);
            Awareness = CharactersUtils.CalculateAwareness(charVm.Awareness, charVm.Equippment);
            Abstract = CharactersUtils.CalculateAbstract(charVm.Abstract, charVm.Equippment);

            // expertise
            Experience = CharactersUtils.CalculateExperience(charVm.Experience, charVm.Equippment);
            DRM = CharactersUtils.CalculateDRM(charVm.DRM, charVm.Equippment);

            // assets
            Health = CharactersUtils.CalculateHealth(Toughness, Strength, Awareness, Experience, EntityLevel, charVm.Equippment);
            Mana = CharactersUtils.CalculateMana(Toughness, Abstract, Experience, EntityLevel, charVm.Equippment);
            Harm = CharactersUtils.CalculateHarm(Strength, Awareness, Experience, EntityLevel, charVm.Equippment);

            Equippment = charVm.Equippment;
            HeroicTraits = charVm.HeroicTraits;
            NegativePerks = charVm.NegativePerks;

            Logbook = charVm.Logbook;
            Supplies = charVm.Supplies;
            Bonuses = charVm.Bonuses;
        }

    }
}
