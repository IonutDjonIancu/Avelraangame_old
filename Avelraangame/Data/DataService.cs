using Avelraangame.Data;
using Avelraangame.Models;
using System.Linq;

namespace Avelraangame.Services
{

    public class DataService
    {
        private AvelraanContext Context { get; set; }
        
        public DataService()
        {
            Context = new AvelraanContext();
        }

        #region Player
        public void SavePlayer(Player player)
        {
            Context.Players.Add(player);
            Context.SaveChanges();
        }
        #endregion

        #region Item
        public void SaveItem(Item item)
        {
            Context.Items.Add(item);
            Context.SaveChanges();
        }

        public int GetItemsCount()
        {
            return Context.Items.Count();
        }
        #endregion

    }
}
