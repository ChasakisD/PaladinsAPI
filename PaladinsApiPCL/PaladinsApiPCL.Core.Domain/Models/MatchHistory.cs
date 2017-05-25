namespace PaladinsApiPCL.Core.Domain.Models
{
    public class MatchHistory : PaladinsResponse
    {
        public int ActiveId1 { get; set; }
        public int ActiveId2 { get; set; }
        public string Active_1 { get; set; }
        public string Active_2 { get; set; }
        public string Active_3 { get; set; }
        public int Assists { get; set; }
        public int Creeps { get; set; }
        public int Damage { get; set; }
        public int Damage_Taken { get; set; }
        public int Deaths { get; set; }
        public string God { get; set; }
        public int GodId { get; set; }
        public int Gold { get; set; }
        public int Healing { get; set; }
        public int ItemId1 { get; set; }
        public int ItemId2 { get; set; }
        public int ItemId3 { get; set; }
        public int ItemId4 { get; set; }
        public int ItemId5 { get; set; }
        public int ItemId6 { get; set; }
        public string Item_1 { get; set; }
        public string Item_2 { get; set; }
        public string Item_3 { get; set; }
        public string Item_4 { get; set; }
        public string Item_5 { get; set; }
        public string Item_6 { get; set; }
        public int Killing_Spree { get; set; }
        public int Kills { get; set; }
        public int Level { get; set; }
        public int Match { get; set; }
        public string Match_Time { get; set; }
        public int Minutes { get; set; }
        public int Multi_kill_Max { get; set; }
        public string Queue { get; set; }
        public string Skin { get; set; }
        public int SkinId { get; set; }
        public string Surrendered { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public string Win_Status { get; set; }
        public string playerName { get; set; }
    }
}
