namespace PaladinsApiPCL.Core.Domain.Models
{
    public class MatchPlayer : PaladinsResponse
    {
        public int Account_Level { get; set; }
        public int GodId { get; set; }
        public string GodName { get; set; }
        public int Mastery_Level { get; set; }
        public int Match { get; set; }
        public string Queue { get; set; }
        public int SkinId { get; set; }
        public int Tier { get; set; }
        public string playerCreated { get; set; }
        public string playerId { get; set; }
        public string playerName { get; set; }
        public int taskForce { get; set; }
        public int tierLosses { get; set; }
        public int tierWins { get; set; }
    }
}
