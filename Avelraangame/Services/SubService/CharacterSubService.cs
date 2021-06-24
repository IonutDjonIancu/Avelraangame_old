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
                PortraitNr = Dice.Roll_min_to_max(1, 7)
            };

            var chr = new Character
            {
                Id = Guid.NewGuid(),
                PlayerId = charVm.PlayerId,

                Name = "Markus",
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
                IsDraft = true,
                Logbook = JsonConvert.SerializeObject(logbook)
            };

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
