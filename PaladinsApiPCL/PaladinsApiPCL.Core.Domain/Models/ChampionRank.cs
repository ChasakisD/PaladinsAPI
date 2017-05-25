namespace PaladinsApiPCL.Core.Domain.Models
{
    public class ChampionRank : PaladinsResponse
    {
        public int Id { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        public int Kills { get; set; }
        public int Losses { get; set; }
        public int MinionKills { get; set; }
        public int Rank { get; set; }
        public int Wins { get; set; }
        public int Worshippers { get; set; }
        public string champion { get; set; }
        public string champion_id { get; set; }
        public string player_id { get; set; }
    }
}
