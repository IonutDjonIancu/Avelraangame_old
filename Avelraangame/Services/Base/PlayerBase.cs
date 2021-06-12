using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Avelraangame.Services.Base
{
    public class PlayerBase
    {
        protected DataService DataService { get; set; }

        protected PlayerBase()
        {
            DataService = new DataService();
        }

        protected PlayerVm ValidateRequestDeserialization_PlayerVm(string request)
        {
            PlayerVm playerVm;

            try
            {
                playerVm = JsonConvert.DeserializeObject<PlayerVm>(request);
            }
            catch (Exception ex)
            {
                throw new Exception(message: string.Concat(Scribe.ShortMessages.BadRequest, ": ", ex.Message));
            }

            return playerVm;
        }

        protected void ValidatePlayerDetails(PlayerVm playerVm)
        {
            ValidateName(playerVm.Name);
            ValidateWard(playerVm.Ward, playerVm.Wardcheck);
        }

        protected void ValidatePlayerUnicity(string name)
        {
            var playerExists = DataService.GetPlayersNames().Any(s => s.Contains(name));

            if (playerExists)
            {
                throw new Exception(message: Scribe.Error_Player_alreadyExists);
            }
        }

        protected void ValidateNumberOfPlayers()
        {
            var numOfPlayers = DataService.GetPlayersCount();

            if (numOfPlayers > 100)
            {
                throw new Exception(message: Scribe.Error_Player_playersLimitReached);
            }
        }



        // privates

        private void ValidateWard(string ward, string wardCheck)
        {
            if (string.IsNullOrWhiteSpace(ward)) { throw new Exception(message: $"Ward {Scribe.Error_AttributeIsMissing}"); }
            if (string.IsNullOrWhiteSpace(wardCheck)) { throw new Exception(message: $"Wardcheck {Scribe.Error_AttributeIsMissing}"); }
            if (ward.Length > 4) { throw new Exception(message: Scribe.Error_Player_wardTooLong); }
            if (!wardCheck.Equals(ward)) { throw new Exception(message: Scribe.Error_Player_wardCheckNotEqualsWard); }
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new Exception(message: $"Name {Scribe.Error_AttributeIsMissing}"); }
            if (name.Length > 50) { throw new Exception(message: Scribe.Error_Player_nameTooLong); }
        }






    }
}
