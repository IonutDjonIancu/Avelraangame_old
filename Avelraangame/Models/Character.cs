using System;

namespace Avelraangame.Models
{
    public partial class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int Strength { get; set; }
        public int Toughness { get; set; }
        public int Awareness { get; set; }
        public int Abstract { get; set; }

        public int Experience { get; set; }
        public int DRM { get; set; }
        public int Wealth { get; set; }

        public int Health { get; set; }
        public int Mana { get; set; }
        public int Harm { get; set; }

        public bool IsAlive { get; set; }
        public string Logbook { get; set; }

        public Player Player { get; set; }
        public Guid PlayerId { get; set; }


    }
}
