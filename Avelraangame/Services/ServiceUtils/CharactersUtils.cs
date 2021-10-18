using Avelraangame.Models.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.ServiceUtils
{
    public static class CharactersUtils
    {
        public enum Races
        {
            Human = 1,
            Elf,
            Dwarf,
            // TODO: add the rest of the races
        }

        public enum Cultures
        {
            Danarian = 1,
            Rovanon,
            Midheim,
            Endarian,
            // TOD: add the rest of the cultures
        }

        #region Stats
        public static int CalculateStrength(int strBase, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToStrength +
                equipp.Mainhand.Bonuses.ToStrength +
                equipp.Offhand.Bonuses.ToStrength +
                equipp.Ranged.Bonuses.ToStrength +
                equipp.Trinkets.Select(s => s.Bonuses.ToStrength).Sum();

            return strBase + itemBonuses;
        }

        public static int CalculateToughness(int touBase, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToToughness +
                equipp.Mainhand.Bonuses.ToToughness +
                equipp.Offhand.Bonuses.ToToughness +
                equipp.Ranged.Bonuses.ToToughness +
                equipp.Trinkets.Select(s => s.Bonuses.ToToughness).Sum();

            return touBase + itemBonuses;
        }

        public static int CalculateAwareness(int awaBase, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToAwareness +
                equipp.Mainhand.Bonuses.ToAwareness +
                equipp.Offhand.Bonuses.ToAwareness +
                equipp.Ranged.Bonuses.ToAwareness +
                equipp.Trinkets.Select(s => s.Bonuses.ToAwareness).Sum();

            return awaBase + itemBonuses;
        }

        public static int CalculateAbstract(int absBase, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToAbstract +
                equipp.Mainhand.Bonuses.ToAbstract +
                equipp.Offhand.Bonuses.ToAbstract +
                equipp.Ranged.Bonuses.ToAbstract +
                equipp.Trinkets.Select(s => s.Bonuses.ToAbstract).Sum();

            return absBase + itemBonuses;
        }
        #endregion
        public static int CalculateExperience(int expBase, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToExperience +
                equipp.Mainhand.Bonuses.ToExperience +
                equipp.Offhand.Bonuses.ToExperience +
                equipp.Ranged.Bonuses.ToExperience +
                equipp.Trinkets.Select(s => s.Bonuses.ToExperience).Sum();

            return expBase + itemBonuses;
        }

        public static int CalculateDRM(int drmBase, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToDRM +
                equipp.Mainhand.Bonuses.ToDRM +
                equipp.Offhand.Bonuses.ToDRM +
                equipp.Ranged.Bonuses.ToDRM +
                equipp.Trinkets.Select(s => s.Bonuses.ToDRM).Sum();

            return drmBase + itemBonuses;
        }
        #region Expertise



        #endregion

        #region Assets
        public static int CalculateHealth (int tou, int str, int awa, int exp, int entityLevel, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToHealth +
               equipp.Mainhand.Bonuses.ToHealth +
               equipp.Offhand.Bonuses.ToHealth +
               equipp.Ranged.Bonuses.ToHealth +
               equipp.Trinkets.Select(s => s.Bonuses.ToHealth).Sum();

            return (5 * tou + 2 * str + awa + exp / 10) * entityLevel + itemBonuses;
        }

        public static int CalculateMana(int tou, int abs, int exp, int entityLevel, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToMana +
               equipp.Mainhand.Bonuses.ToMana +
               equipp.Offhand.Bonuses.ToMana +
               equipp.Ranged.Bonuses.ToMana +
               equipp.Trinkets.Select(s => s.Bonuses.ToMana).Sum();

            return ((2 * tou + exp / 5) * entityLevel + itemBonuses) * abs / 10;
        }

        public static int CalculateHarm(int str, int awa, int exp, int entityLevel, Equipment equipp)
        {
            var itemBonuses = equipp.Armour.Bonuses.ToHarm +
               equipp.Mainhand.Bonuses.ToHarm +
               equipp.Offhand.Bonuses.ToHarm +
               equipp.Ranged.Bonuses.ToHarm +
               equipp.Trinkets.Select(s => s.Bonuses.ToHarm).Sum();

            return (2 * str + 2 * awa + exp / 2) * entityLevel + itemBonuses;
        }
        #endregion
    }
}
