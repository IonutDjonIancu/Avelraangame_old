using System;

namespace Avelraangame.Models.ViewModels
{
    public class PlayerVm
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string Ward { get; set; }
        public string Wardcheck { get; set; }

        public PlayerVm(Player player)
        {
            PlayerId = player.Id;
            PlayerName = player.Name;
        }

        public PlayerVm()
        {

        }


    }
}
