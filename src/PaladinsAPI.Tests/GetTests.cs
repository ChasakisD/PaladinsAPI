using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PaladinsAPI.Enumerations;

namespace PaladinsAPI.Tests
{
    [TestClass]
    public class GetTests
    {
        private readonly PaladinsApi _api =
            new PaladinsApi("", "", Platform.Pc);

        private readonly string _playerName = "TryhardPants";
        private int _playerId;
        private readonly int _matchId = 117838396;

        [TestInitialize]
        public async Task GetPlayerId()
        {
            var player = await _api.GetPlayer(_playerName);

            Assert.IsNotNull(player);

            _playerId = player.Id;
        }

        [TestMethod]
        public async Task CanGetChampionsAsync()
        {
            var champions = await _api.GetChampions(eLanguageCode.English);
            var test = JsonConvert.SerializeObject(champions);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task CanGetChampionRanksByName()
        {
            var response = await _api.GetChampionRanks(_playerName);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetChampionRanksById()
        {
            var response = await _api.GetChampionRanks(_playerId);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetChampionSkins()
        {
            var response = await _api.GetChampionSkins((int)Champions.Androxus, eLanguageCode.English);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetDataUsed()
        {
            var response = await _api.GetDataUsed();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetFriendsById()
        {
            var response = await _api.GetFriends(_playerId);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetFriendsByName()
        {
            var response = await _api.GetFriends(_playerName);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetItems()
        {
            var response = await _api.GetItems(eLanguageCode.English);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetMatchHistoryById()
        {
            var response = await _api.GetMatchHistory(_playerId);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetMatchHistoryByName()
        {
            var response = await _api.GetMatchHistory(_playerName);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task CanGetMatchDetails()
        {
            var response = await _api.GetMatchDetails(_matchId);

            Assert.IsNotNull(response);
        }
    }
}
