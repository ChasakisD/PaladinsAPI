namespace PaladinsApiPCL.Core.Domain.Models
{
    public class QueueChampionStat : PaladinsResponse
    {
        public int Assists { get; set; }
        public string Champion { get; set; }
        public int ChampionId { get; set; }
        public int Deaths { get; set; }
        public int Gold { get; set; }
        public int Kills { get; set; }
        public string LastPlayed { get; set; }
        public int Losses { get; set; }
        public int Matches { get; set; }
        public int Minutes { get; set; }
        public string Queue { get; set; }
        public int Wins { get; set; }
        public string player_id { get; set; }
    }
}
