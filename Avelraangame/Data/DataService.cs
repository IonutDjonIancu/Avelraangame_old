﻿using Avelraangame.Data;
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
        public void SaveItem(Item item)
        {
            Context.Items.Add(item);
            Context.SaveChanges();
        }

        public int GetItemsCount()
        {
            return Context.Items.Count();
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

        #endregion

        #region Character
        public void SaveCharacter(Character chr)
        {
            Context.Characters.Add(chr);
            Context.SaveChanges();
        }

        public Character GetCharacterById(Guid charId)
        {
            return Context.Characters
                .Where(s => s.Id == charId)
                .FirstOrDefault();
        }

        public List<Character> GetCharactersByPlayerId(Guid playerId)
        {
            return Context.Characters
                .Where(s => s.PlayerId.Equals(playerId))
                .ToList();
        }


        public List<Character> GetCharactersDraftByPlayerId(Guid playerId)
        {
            return Context.Characters
                .Where(s => s.PlayerId.Equals(playerId) & s.IsDraft.Equals(true))
                .ToList();
        }

        #endregion
    }
}
