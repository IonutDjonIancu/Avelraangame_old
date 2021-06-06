using System;
using System.ComponentModel.DataAnnotations;

namespace Avelraangame.Models
{
    public partial class Character
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string StatsBase { get; set; }

        public string AssetsBase { get; set; }

        public bool IsAlive { get; set; }


        [Required]
        public Player Player { get; set; }
        public Guid PlayerId { get; set; }

        //public Guid LocationId { get; set; }

    }
}
