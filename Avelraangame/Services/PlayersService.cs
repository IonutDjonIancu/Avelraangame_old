using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Avelraangame.Services.Validations;
using Newtonsoft.Json;

namespace Avelraangame.Services
{
    public class PlayersService : PlayersSubService
    {
        public PlayerValidations PlayerValidations { get; set; }
        public DataService DataService { get; set; }

        public PlayersService()
        {
            DataService = new DataService();
            PlayerValidations = new PlayerValidations();
        }

        public Scribe.ShortMessages CreatePlayer(RequestVm request)
        {
            var playerVm = JsonConvert.DeserializeObject<PlayerVm>(request.Message);

            


            // insert validations



            //Context.Players.Add(player); <--------------------------- must be moved in DataService
            //Context.SaveChanges();

            return Scribe.ShortMessages.Success;
        }
    }
}
