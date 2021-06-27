using Avelraangame.Models;
using Avelraangame.Models.ModelScraps;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avelraangame.Services.SubService
{

    public class CharacterSubService : CharacterBase
    {
        private TempsService Temps { get; set; }
        private ItemsService Items { get; set; }

        public CharacterSubService()
        {
            Temps = new TempsService();
            Items = new ItemsService();
        }

        protected int GetEntityLevelByRoll(int roll)
        {
            if (roll < 20)
            {
                return 1;
            }
            else if (roll >= 20 && roll < 40)
            {
                return 2;
            }
            else if (roll >= 40 && roll < 60)
            {
                return 3;
            }
            else if (roll >= 60 && roll < 80)
            {
                return 4;
            }
            else if (roll >= 80 && roll < 100)
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }

        protected Character CreateHumanCharacter(CharacterVm charVm)
        {
            var roll = charVm.Logbook.StatsRoll;

            var logbook = new Logbook
            {
                StatsRoll = roll,
                ItemsRoll = Dice.Roll_min_to_max(2, 12),
                PortraitNr = Dice.Roll_min_to_max(1, 7)
            };

            var chr = new Character
            {
                Id = Guid.NewGuid(),
                PlayerId = charVm.PlayerId,

                Name = ValidateCharacterName(charVm.Name),
                Race = (int)CharactersUtils.Races.Human,
                Culture = (int)CharactersUtils.Cultures.Danarian,

                Strength = 10,
                Toughness = 10,
                Awareness = 10,
                Abstract = 10,

                EntityLevel = GetEntityLevelByRoll(roll),
                Experience = 0,
                DRM = 0,
                Wealth = 0,

                Health = 10,
                Mana = 0,
                Harm = 1,

                IsAlive = true,
                HasLevelup = true,
                Logbook = JsonConvert.SerializeObject(logbook)
            };

            var trinks = new List<ItemVm>();

            for (int i = 0; i < 6; i++)
            {
                var a = Items.GenerateRandomItem();
                trinks.Add(a);
            }

            var equipp = new Equippment
            {
                Armour = Items.GenerateRandomItem(),
                Mainhand = Items.GenerateRandomItem(),
                Offhand = Items.GenerateRandomItem(),
                Ranged = Items.GenerateRandomItem(),
                Trinkets = trinks,
            };

            var itemsInSupplies = new List<ItemVm>();

            for (int i = 0; i < logbook.ItemsRoll; i++)
            {
                var item = Items.GenerateRandomItem(chr.Id.ToString());
                itemsInSupplies.Add(item);
            }

            chr.Equippment = JsonConvert.SerializeObject(equipp);
            chr.Supplies = JsonConvert.SerializeObject(itemsInSupplies);

            return chr;
        }

        protected Character ModifyHumanCharacter(CharacterVm charVm)
        {


            // calculate, 
            // validate and 
            // apply modifications before return



            var roll = charVm.Logbook.StatsRoll;

            var logbook = new Logbook
            {
                StatsRoll = roll,
            };

            var chr = new Character
            {
                Id = Guid.NewGuid(),
                PlayerId = charVm.PlayerId,
                Name = null,

                Strength = 10,
                Toughness = 10,
                Awareness = 10,
                Abstract = 10,

                EntityLevel = GetEntityLevelByRoll(roll),
                Experience = 0,
                DRM = 0,
                Wealth = 0,

                Health = 10,
                Mana = 0,
                Harm = 1,

                IsAlive = true,
                Logbook = JsonConvert.SerializeObject(logbook)
            };

            return chr;
        }




    }

}
