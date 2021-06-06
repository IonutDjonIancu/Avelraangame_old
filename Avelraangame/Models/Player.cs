using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Avelraangame.Models
{
    public partial class Player
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Ward { get; set; }

        public ICollection<Character> Characters { get; set; }
    }
}
