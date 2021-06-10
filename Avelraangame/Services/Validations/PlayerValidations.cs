using Avelraangame.Models;
using Avelraangame.Services.ServiceUtils;
using System;

namespace Avelraangame.Services.Validations
{
    public class PlayerValidations
    {
        public void ValidatePlayer(Player player, string wardCheck)
        {
            ValidateName(player.Name);
            ValidateWard(player.Ward, wardCheck);

        }


        private void ValidateWard(string ward, string wardCheck)
        {
            if (string.IsNullOrWhiteSpace(ward)) throw new Exception(message: $"{ward} {Scribe.Error_Validation_attributeIsMissing}");
            if (string.IsNullOrWhiteSpace(wardCheck)) throw new Exception(message: $"{wardCheck} {Scribe.Error_Validation_attributeIsMissing}");
            if (ward.Length >= 10) throw new Exception(message: Scribe.Error_Validation_Player_wardTooLong);
            if (!wardCheck.Equals(ward)) throw new Exception(message: Scribe.Error_Validation_Player_wardCheckNotEqualsWard);
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception(message: $"{name} {Scribe.Error_Validation_attributeIsMissing}");
            if (name.Length >= 50) throw new Exception(message: Scribe.Error_Validation_Player_nameTooLong);
        }






    }
}
