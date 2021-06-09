using Avelraangame.Models.ApiModels;
using Avelraangame.Services;
using Avelraangame.Services.Validations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Avelraangame.Controllers
{
    //[Route()]
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
            var request = itemService.GenerateRandomItem();

            if (request.OperationSuccess)
            {
                return request.Response;
            }
            else
            {
                return request.Message;
            }
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


        // GET: palantir/CreateItem
        [HttpGet("CreateItem")]
        public void CreateItem()
        {
            var itemService = new ItemsService();

            itemService.CreateItem();
        }
        #endregion


        #region player
        // POST: palantir/createplayer
        [HttpPost("CreatePlayer")]
        public IActionResult CreatePlayer(RequestModel request)
        {
            var (statusCode, statusMessage) = PalantirValidations.ValidateRequest(request);

            if (statusCode != 200) 
            {
                return StatusCode(statusCode, statusMessage);
            }

            var playerService = new PlayersService();
            var isSuccess = playerService.CreatePlayer(request);

            return StatusCode(200, "Player created");
        }
        #endregion

    }
}
