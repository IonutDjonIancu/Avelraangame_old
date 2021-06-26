using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.Base
{
    public class ItemBase
    {
        protected DataService DataService { get; set; }

        protected ItemBase()
        {
            DataService = new DataService();
        }

        protected void ValidateCharacterId(Guid charId)
        {
            if (charId.Equals(Guid.Empty) || charId == null)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.ResourceNotFound, ": characterId is missing or invalid."));
            }
        }


    }
}
