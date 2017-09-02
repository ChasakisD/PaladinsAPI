using System.Collections.Generic;

namespace PaladinsAPI.Models
{
    public class PlayerLoadouts : PaladinsResponse
    {
        public int ChampionId { get; set; }
        public string ChampionName { get; set; }
        public int DeckId { get; set; }
        public string DeckName { get; set; }
        public List<LoadoutItem> LoadoutItems { get; set; }
        public int playerId { get; set; }
        public string playerName { get; set; }
    }
}
