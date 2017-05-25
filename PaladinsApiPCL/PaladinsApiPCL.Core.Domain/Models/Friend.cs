namespace PaladinsApiPCL.Core.Domain.Models
{
    public class Friend : PaladinsResponse
    {
        public string account_id { get; set; }
        public string name { get; set; }
        public string player_id { get; set; }
    }
}
