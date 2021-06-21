﻿using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;

namespace Avelraangame.Services.Base
{
    public static class PalantirBase
    {
        public static Scribe.ShortMessages ValidateRequest(RequestVm request)
        {
            if (request == null)
            {
                return Scribe.ShortMessages.BadRequest;
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return Scribe.ShortMessages.BadRequest;
            }

            return Scribe.ShortMessages.Ok;
        }
    }
}