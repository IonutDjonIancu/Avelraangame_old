using Avelraangame.Models.ModelScraps;
using System;

namespace Avelraangame.Models.ViewModels
{
    public class CharacterVm
    {
        public Guid CharacterId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }

        public string Name { get; set; }
        public bool IsAlive { get; set; }

        public int Strength { get; set; }
        public int Toughness { get; set; }
        public int Awareness { get; set; }
        public int Abstract { get; set; }

        public int Experience { get; set; }
        public int DRM { get; set; }
        public int Wealth { get; set; }

        public int Harm { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }

        public Logbook Logbook { get; set; }

        public CharacterVm()
        {
            Logbook = new Logbook();
        }
    }
}
