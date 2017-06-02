using Newtonsoft.Json;
using PaladinsApiPCL.Core.Domain.Enumerations;
using System.Threading.Tasks;
using Xunit;

namespace PaladinsApiPCL.Core.Tests
{
    public class GetTests
    {
        public readonly string devId = "";
        public readonly string authKey = "";
        public readonly string playerName = "dawgeth";
        public readonly int playerId = 1178604;
        public readonly int matchId = 117838396;

        [Fact]
        public async Task CanGetChampionsAsync()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var champions = await PaladinsAPI.GetChampions(eLanguageCode.English);

            var test = JsonConvert.SerializeObject(champions);

            Assert.NotNull(champions);
        }

        [Fact]
        public async Task CanGetChampionRanksByName()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetChampionRanks(playerName);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetChampionRanksById()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetChampionRanks(playerId);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetChampionSkins()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetChampionSkins((int)Champions.Androxus, eLanguageCode.English);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetDataUsed()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetDataUsed();

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetFriendsById()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetFriends(playerId);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetFriendsByName()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetFriends(playerName);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetItems()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetItems(eLanguageCode.English);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetMatchHistoryById()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetMatchHistory(playerId);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetMatchHistoryByName()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetMatchHistory(playerName);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CanGetMatchDetails()
        {
            PaladinsAPI.Initializer(devId, authKey);

            var response = await PaladinsAPI.GetMatchDetails(matchId);

            Assert.NotNull(response);
        }
    }
}
