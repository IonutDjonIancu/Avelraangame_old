using Avelraangame.Models;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.Base
{
    public class TempsBase
    {
        protected DataService DataService { get; set; }

        protected TempsBase()
        {
            DataService = new DataService();
        }

        protected void ValidateTempInfo(TempInfo info)
        {
            if (info.CharacterId.Equals(Guid.Empty) || info.CharacterId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": characterId is missing."));
            }

            if (info.BonusTo == 0)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": bonus info is missing."));
            }

            if (info.Value < 1)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": non negative values accepted only."));
            }
        }

    }
}
