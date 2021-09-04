using Avelraangame.Models.POCOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Avelraangame.Models.ViewModels
{
    public class CharacterVm
    {
        public Guid CharacterId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }

        public string Name { get; set; }

        public Stats Stats { get; set; }
        public Assets Assets { get; set; }
        public Expertise Expertise { get; set; }
        public Skills Skills { get; set; }

        public bool IsAlive { get; set; }
        public bool HasLevelup { get; set; }
        public bool AttackToken { get; set; }
        public bool InParty { get; set; }
        public Guid? PartyId { get; set; }

        public bool? InFight { get; set; }
        public Guid? FightId { get; set; }

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
            AttackToken = true;
        }

        public CharacterVm(Character chr)
        {
            CharacterId = chr.Id;
            PlayerId = chr.PlayerId.GetValueOrDefault();
            PlayerName = chr.Player?.Name;
            Name = chr.Name;

            IsAlive = chr.IsAlive;
            HasLevelup = chr.HasLevelup;
            InParty = chr.InParty;
            PartyId = chr.PartyId;
            InFight = chr.InFight;
            FightId = chr.FightId;
            AttackToken = true;

            if (!string.IsNullOrWhiteSpace(chr.Stats))
            {
                Stats = JsonConvert.DeserializeObject<Stats>(chr.Stats);
            }
            if (!string.IsNullOrWhiteSpace(chr.Expertise))
            {
                Expertise = JsonConvert.DeserializeObject<Expertise>(chr.Expertise);
            }
            if (!string.IsNullOrWhiteSpace(chr.Assets))
            {
                Assets = JsonConvert.DeserializeObject<Assets>(chr.Assets);
            }
            if (!string.IsNullOrWhiteSpace(chr.Skills))
            {
                Skills = JsonConvert.DeserializeObject<Skills>(chr.Skills);
            }
            if (!string.IsNullOrWhiteSpace(chr.Logbook))
            {
                Logbook = JsonConvert.DeserializeObject<Logbook>(chr.Logbook);
            }
            
            if (!string.IsNullOrWhiteSpace(chr.Equippment))
            {
                Equippment = JsonConvert.DeserializeObject<Equippment>(chr.Equippment);
            }

            if (!string.IsNullOrWhiteSpace(chr.HeroicTraits))
            {
                HeroicTraits = JsonConvert.DeserializeObject<List<HeroicTraits>>(chr.HeroicTraits);
            }

            if (!string.IsNullOrWhiteSpace(chr.NegativePerks))
            {
                NegativePerks = JsonConvert.DeserializeObject<List<NegativePerks>>(chr.NegativePerks);
            }

            if (!string.IsNullOrWhiteSpace(chr.Supplies))
            {
                Supplies = JsonConvert.DeserializeObject<List<ItemVm>>(chr.Supplies);
            }

        }
    }
}
