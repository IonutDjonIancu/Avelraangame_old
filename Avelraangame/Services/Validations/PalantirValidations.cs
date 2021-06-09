using Avelraangame.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avelraangame.Services.Validations
{
    public static class PalantirValidations
    {
        public static (int statusCode, string statusMessage) ValidateRequest(RequestModel request)
        {
            if (request == null)
            {
                return (statusCode: 403, statusMessage: "Request is null");
            }
            if (request.RequestPayload == null)
            {
                return (statusCode: 403, statusMessage: "Request payload missing or null");
            }

            return (statusCode: 200, statusMessage: "Ok");
        }
    }
}
