using System;

namespace PaladinsApiPCL.Core.Domain.Models
{
    public class Player : PaladinsResponse
    {
        public string Created_Datetime { get; set; }
        public int Id { get; set; }
        public string Last_Login_Datetime { get; set; }
        public int Leaves { get; set; }
        public int Level { get; set; }
        public int Losses { get; set; }
        public int MasteryLevel { get; set; }
        public string Name { get; set; }
        public string Personal_Status_Message { get; set; }
        public string Region { get; set; }
        public int TeamId { get; set; }
        public string Team_Name { get; set; }
        public int Total_Achievements { get; set; }
        public int Total_Worshippers { get; set; }
        public int Wins { get; set; }
        public DateTime lastUpdated { get; set; }
        public DateTime Champions_Last_Updated { get; set; }
        public DateTime History_Last_Updated { get; set; }
    }
}
