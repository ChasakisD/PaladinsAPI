namespace PaladinsAPI.Models
{
    public class PlayerStatus : PaladinsResponse
    {
        public int Match { get; set; }
        public string personal_status_message { get; set; }
        public int status { get; set; }
        public string status_string { get; set; }
        public int playerId { get; set; }
    }
}
