using Avelraangame.Data;
using Avelraangame.Models;
using System.Collections.Generic;
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
        public Player GetPlayerByName(string name)
        {
            return Context.Players.Where(s => s.Name == name).FirstOrDefault();
        }

        public void SavePlayer(Player player)
        {
            Context.Players.Add(player);
            Context.SaveChanges();
        }
        
        public List<string> GetPlayersNames()
        {
            return Context.Players.Select(s => s.Name).ToList();
        }

        public int GetPlayersCount()
        {
            return Context.Players.Count();
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
