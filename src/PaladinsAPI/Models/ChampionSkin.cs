namespace PaladinsAPI.Models
{
    public class ChampionSkin : PaladinsResponse
    {
        public int champion_id { get; set; }
        public string champion_name { get; set; }
        public int skin_id1 { get; set; }
        public int skin_id2 { get; set; }
        public string skin_name { get; set; }
    }
}
