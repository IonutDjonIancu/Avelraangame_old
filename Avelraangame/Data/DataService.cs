using Avelraangame.Data;
using Avelraangame.Models;
using Avelraangame.Services.ServiceUtils;
using System;
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
        public bool PlayerExists(string playerName)
        {
            return Context.Players.Where(s => s.Name.Equals(playerName)).Any();
        }

        public Player GetPlayerByName(string name)
        {
            return Context.Players.Where(s => s.Name == name).FirstOrDefault();
        }

        public Player GetPlayerById(Guid id)
        {
            return Context.Players.Where(s => s.Id == id).FirstOrDefault();
        }

        public void SavePlayer(Player player)
        {
            Context.Players.Add(player);
            Context.SaveChanges();
        }
        
        public List<string> GetPlayersNames()
        {
            return Context.Players.OrderBy(s => s.Name).Select(s => s.Name).ToList();
        }

        public int GetPlayersCount()
        {
            return Context.Players.Count();
        }
        #endregion

        #region Item
        public void CreateItem(Item item)
        {
            Context.Items.Add(item);
            Context.SaveChanges();
        }

        public void UpdateItem(Item item)
        {
            Context.Items.Update(item);
            Context.SaveChanges();
        }

        public int GetItemsCount()
        {
            return Context.Items.Count();
        }

        public Item GetItemById(Guid itemId)
        {
            return Context.Items.Find(itemId);
        }

        public List<Item> GetSuppliesItemsByCharacterId(Guid charId)
        {
            return Context.Items
                .Where(s => s.CharacterId == charId & s.IsEquipped == false)
                .ToList();
        } 

        public List<Item> GetEquippedItemsByCharId(Guid charId)
        {
            return Context.Items
                .Where(s => s.CharacterId == charId & s.IsEquipped == true)
                .ToList();
        }

        #endregion

        #region Temps
        public void SaveTempCharacterInfo(TempInfo temps)
        {
            Context.Temps.Add(temps);
            Context.SaveChanges();
        }

        public List<TempInfo> GetTempInfosByCharacterId(Guid charId)
        {
            return Context.Temps.Where(s => s.CharacterId == charId).ToList();
        } 

        public void RemoveTempsInfo(List<TempInfo> temps)
        {
            Context.RemoveRange(temps);
            Context.SaveChanges();
        }

        #endregion

        #region Character

        public List<Character> GetCharacters()
        {
            return Context.Characters
                .ToList();
        }

        public void SaveCharacter(Character chr)
        {
            Context.Characters.Add(chr);
            Context.SaveChanges();
        }

        public void UpdateCharacter(Character chr)
        {
            Context.Characters.Update(chr);
            Context.SaveChanges();
        }

        public Character GetCharacterById(Guid charId)
        {
            return Context.Characters
                .Where(s => s.Id.Equals(charId))
                .FirstOrDefault();
        }

        public List<Character> GetCharactersByPlayerId(Guid playerId)
        {
            return Context.Characters
                .Where(s => s.PlayerId.Equals(playerId) && s.IsAlive)
                .ToList();
        }


        public List<Character> GetCharactersDraftByPlayerId(Guid playerId)
        {
            return Context.Characters
                .Where(s => s.PlayerId.Equals(playerId) & s.HasLevelup.Equals(true))
                .ToList();
        }

        #endregion

        #region Party
        public void CreateParty(Party party)
        {
            Context.Party.Add(party);
            Context.SaveChanges();
        }
        #endregion

        #region Storage
        public void CreateStorage(Storage store)
        {
            Context.Storage.Add(store);
            Context.SaveChanges();
        }

        public void UpdateStorage(Storage store)
        {
            Context.Storage.Update(store);
            Context.SaveChanges();
        }

        public void DeleteStorage(Storage store)
        {
            Context.Storage.Remove(store);
            Context.SaveChanges();
        }

        public string GetStorageValueById(Guid storageId)
        {
            return Context
                .Storage
                .Where(s => s.Id.Equals(storageId))
                .FirstOrDefault()
                ?.Value;
        }

        public Storage GetStorage(Guid storageId)
        {
            return Context
                .Storage
                .Where(s => s.Id.Equals(storageId))
                .FirstOrDefault();
        }
        #endregion
    }
}
