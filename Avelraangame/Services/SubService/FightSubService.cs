using Avelraangame.Models;
using Avelraangame.Models.POCOs;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.Base;
using Avelraangame.Services.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avelraangame.Services.SubService
{
    public class FightSubService : ServiceBase
    {
        protected void UlockCharactersFromFight(List<Character> characters)
        {
            foreach (var chr in characters)
            {
                chr.IsInFight = false;
                chr.FightId = null;
                DataService.UpdateCharacter(chr);
            }
        }

        protected void LockCharactersInFight(List<Character> characters, Guid fightId)
        {
            foreach (var chr in characters)
            {
                chr.IsInFight = true;
                chr.FightId = fightId;
                DataService.UpdateCharacter(chr);
            }
        }

        protected (List<CharacterVm> goodGuys, int minHitMarker, int maxHitMarker) GetTheGoodGuys(List<Character> characters)
        {
            var charactersService = new CharactersService();
            var goodGuys = new List<CharacterVm>();

            foreach (var chr in characters)
            {
                var character = charactersService.GetCalculatedCharacterById(chr.Id);
                goodGuys.Add(character);
            }

            var (minHitMarker, maxHitMarker) = GetSkillsHitMarkers(goodGuys);

            return (goodGuys, minHitMarker, maxHitMarker);
        }

        protected (List<CharacterVm> badGuys, string partySize) GetTheBadGuys(int minHitMarker, int maxHitmarker, Guid fightId)
        {
            var characterService = new CharactersService();
            var (numberOfBadGuys, partySize) = GetPartySize();
            var badGuys = new List<CharacterVm>();

            for (int i = 0; i < numberOfBadGuys; i++)
            {
                badGuys.Add(characterService.GenerateNPC(minHitMarker, maxHitmarker, fightId));
            }

            return (badGuys, partySize);
        }

        protected FightDetails GetTheFightDetails(List<CharacterVm> badGuys, string partySize)
        {
            var loot = new List<ItemVm>();

            foreach (var chr in badGuys)
            {
                loot.Add(chr.Equippment.Mainhand);
                loot.Add(chr.Equippment.Offhand);
                loot.Add(chr.Equippment.Armour);
                loot.Add(chr.Equippment.Ranged);

                foreach (var item in chr.Equippment.Trinkets)
                {
                    loot.Add(item);
                }
            }
            
            return new FightDetails
            {
                Difficulty = partySize,
                StartDate = DateTime.Now,
                Loot = loot
            };
        }

        private (int min, int max) GetSkillsHitMarkers(List<CharacterVm> chars)
        {
            var hitMarkers = new List<int>();

            foreach (var chr in chars)
            {
                int[] skillsArr = { chr.Skills.Melee, chr.Skills.Ranged, chr.Skills.Hide, chr.Skills.Arcane, chr.Skills.Unarmed };
                hitMarkers.Add(skillsArr.Max());
            }

            return (hitMarkers.Min() / 2, hitMarkers.Max());
        }

        private (int, string) GetPartySize()
        {
            var roll = Dice.Roll_min_to_max(1, 100);

            if (roll <= 50)
            {
                return (Dice.Roll_min_to_max(2, 5), Scribe.PartySize.party); 
            }
            else if (roll > 50 && roll <= 75)
            {
                return (Dice.Roll_min_to_max(3, 8), Scribe.PartySize.group);
            }
            else if (roll > 75 && roll <= 90)
            {
                return (Dice.Roll_min_to_max(6, 12), Scribe.PartySize.band);
            }
            else if (roll > 90 && roll <= 99)
            {
                return (Dice.Roll_min_to_max(10, 20), Scribe.PartySize.company);
            }
            else
            {
                return (Dice.Roll_min_to_max(20, 50), Scribe.PartySize.troops);
            }


        }

    }
}
