namespace PaladinsApiPCL.Core.Domain.Models
{
    public class TopMatch : PaladinsResponse
    {
        public string Ban1 { get; set; }
        public int Ban1Id { get; set; }
        public string Ban2 { get; set; }
        public int Ban2Id { get; set; }
        public string Entry_Datetime { get; set; }
        public int LiveSpectators { get; set; }
        public int Match { get; set; }
        public int Match_Time { get; set; }
        public int OfflineSpectators { get; set; }
        public string Queue { get; set; }
        public string RecordingFinished { get; set; }
        public string RecordingStarted { get; set; }
        public int Team1_AvgLevel { get; set; }
        public int Team1_Gold { get; set; }
        public int Team1_Kills { get; set; }
        public int Team1_Score { get; set; }
        public int Team2_AvgLevel { get; set; }
        public int Team2_Gold { get; set; }
        public int Team2_Kills { get; set; }
        public int Team2_Score { get; set; }
        public int WinningTeam { get; set; }
    }
}
