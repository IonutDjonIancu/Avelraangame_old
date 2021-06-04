using Avelraangame.Services;
using Avelraangame.Services.ServiceUtils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Avelraangame.Controllers
{
    public class PalantirController : ControllerBase
    {
        // GET: palantir/GetOk
        [HttpGet]
        public string GetOk()
        {
            return "ok";
        }

        // GET: palantir/GenerateItem
        [HttpGet]
        public string GenerateItem()
        {
            var itemService = new ItemsService();
            var item = itemService.GenerateRandomItem();

            var result = JsonConvert.SerializeObject(item);

            return result;
        }

        // GET: palantir/GetString
        [HttpGet]
        public string GetString()
        {
            var result = Scribe.Stats_Strength;


            return result;
        }

        // GET: palantir/GetItems
        [HttpGet]
        public string GetItems()
        {
            var itemService = new ItemsService();
            var listOfItems = itemService.GetItems();

            var result = JsonConvert.SerializeObject(listOfItems);

            return result;
        }


        // GET: palantir/CreateItem
        [HttpGet]
        public void CreateItem()
        {
            var itemService = new ItemsService();

            itemService.CreateItem();
        }

    }
}
