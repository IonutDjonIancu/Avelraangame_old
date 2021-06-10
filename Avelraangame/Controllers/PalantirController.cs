using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.Validations;
using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers
{
    [Route("api/palantir")]
    [ApiController]
    public class PalantirController : ControllerBase
    {
        // GET: palantir/GetOk
        [HttpGet("GetOk")]
        public string GetOk()
        {
            return Scribe.ShortMessages.Ok.ToString();
        }

        #region Items
        // GET: palantir/GenerateItem
        [HttpGet("GenerateItem")]
        public string GenerateItem()
        {
            var itemService = new ItemsService();
            return itemService.GenerateRandomItem();
        }

        // GET: palantir/GetItems
        [HttpGet("GetItemsCount")]
        public int GetItemsCount()
        {
            var itemService = new ItemsService();
            return itemService.GetItemsCount();
        }
        #endregion


        #region player
        // POST: palantir/createplayer
        [HttpPost("CreatePlayer")]
        public string CreatePlayer([FromBody]RequestVm request)
        {
            var validateRequest = PalantirValidations.ValidateRequest(request);

            if (validateRequest.Equals(Scribe.ShortMessages.BadRequest))
            {
                return validateRequest.ToString();
            }

            var playerService = new PlayersService();
            var response = playerService.CreatePlayer(request);

            return response.ToString();
        }
        #endregion

    }
}
