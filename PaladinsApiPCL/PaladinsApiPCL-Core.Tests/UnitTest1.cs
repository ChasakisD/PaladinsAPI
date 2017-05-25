using System;
using System.Threading.Tasks;
using Xunit;

namespace PaladinsApiPCL_Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task CanGetChampionsAsync()
        {
            var devId = "DEVID";
            var authKey = "AUTHKEY";
            PaladinsAPI.Initializer(devId, authKey);

            var champions = await PaladinsAPI.GetChampions(PaladinsAPI.eLanguageCode.English);

            Assert.NotNull(champions);
        }
    }
}
