﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Models.ViewModels
{
    public class CharacterVm
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int Roll { get; set; }
    }
}
