using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Avelraangame.Controllers
{
    [Route("api/palantir")]
    [ApiController]
    public class PalantirController : ControllerBase
    {
        // GET: api/palantir/GetOk
        [HttpGet("GetOk")]
        public string GetOk()
        {
            return Scribe.ShortMessages.Ok.ToString();
        }

    #region Items
        // GET: api/palantir/GenerateItem
        [HttpGet("GenerateItem")]
        public string GenerateItem()
        {
            var itemService = new ItemsService();
            return itemService.GenerateRandomItem();
        }

        // GET: api/palantir/GetItems
        [HttpGet("GetItemsCount")]
        public int GetItemsCount()
        {
            var itemService = new ItemsService();
            return itemService.GetItemsCount();
        }
    #endregion


    #region Players
        // POST: api/palantir/createplayer
        [HttpPost("CreatePlayer")]
        public string CreatePlayer([FromBody]RequestVm request)
        {
            var validateRequest = PalantirBase.ValidatePOSTRequest(request);

            if (validateRequest.Equals(Scribe.ShortMessages.BadRequest))
            {
                return validateRequest.ToString();
            }

            var playerService = new PlayersService();
            return playerService.CreatePlayer(request);
        }
    #endregion

    }
}
