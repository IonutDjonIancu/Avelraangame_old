using Avelraangame.Data;
using Avelraangame.Models;

namespace Avelraangame.Services
{

    public class DataService
    {
        private AvelraanContext Context { get; set; }
        
        public DataService()
        {
            Context = new AvelraanContext();
        }

        public void CreatePlayer(Player player)
        {
            Context.Players.Add(player);
            Context.SaveChanges();
        }



    }
}
