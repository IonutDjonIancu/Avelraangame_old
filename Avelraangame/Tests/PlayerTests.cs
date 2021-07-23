using Avelraangame.Models.ViewModels;
using Avelraangame.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Avelraangame.Tests
{
    public class PlayerTests
    {
        [Test]
        public void CreatePlayer()
        {
            var name = "CreateTestPlayer";
            var ward = "1234";
            var wardCheck = "1234";
            var playerService = new PlayersService();

            var playerVm = new PlayerVm()
            {
                PlayerName = name,
                Ward = ward,
                Wardcheck = wardCheck
            };

            var requestVm = new RequestVm()
            {
                Message = JsonConvert.SerializeObject(playerVm)
            };

            playerService.CreatePlayer(requestVm);

            Assert.That(() => playerService.GetPlayerIdByName(requestVm).Equals(name));

        }
    }
}
