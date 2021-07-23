using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.Base
{
    public static class PalantirBase
    {
        public static void ValidateRequest(RequestVm request)
        {
            if (request.Equals(null))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": request is in bad format."));
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": request message is missing."));
            }
        }
    }
}
