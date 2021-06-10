using Avelraangame.Models.ApiModels;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.Validations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            return "ok";
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
        [HttpGet("GetItems")]
        public string GetItems()
        {
            var itemService = new ItemsService();
            var listOfItems = itemService.GetItems();

            var result = JsonConvert.SerializeObject(listOfItems);

            return result;
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
