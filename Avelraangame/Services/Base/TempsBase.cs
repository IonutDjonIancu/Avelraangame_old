using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.Base
{
    public class TempsBase
    {
        protected DataService DataService { get; set; }

        protected TempsBase()
        {
            DataService = new DataService();
        }


    }
}
