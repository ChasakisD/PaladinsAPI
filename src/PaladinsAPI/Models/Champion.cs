namespace PaladinsAPI.Models
{
    public class Champion : PaladinsResponse
    {
        public int id { get; set; }
        public string Ability1 { get; set; }
        public string Ability2 { get; set; }
        public string Ability3 { get; set; }
        public string Ability4 { get; set; }
        public string Ability5 { get; set; }
        public int abilityId1 { get; set; }
        public int abilityId2 { get; set; }
        public int abilityId3 { get; set; }
        public int abilityId4 { get; set; }
        public int abilityId5 { get; set; }
        public string ChampionAbility1_URL { get; set; }
        public string ChampionAbility2_URL { get; set; }
        public string ChampionAbility3_URL { get; set; }
        public string ChampionAbility4_URL { get; set; }
        public string ChampionAbility5_URL { get; set; }
        public string ChampionCard_URL { get; set; }
        public string ChampionIcon_URL { get; set; }
        public string Cons { get; set; }
        public int Health { get; set; }
        public string Lore { get; set; }
        public string Name { get; set; }
        public string OnFreeRotation { get; set; }
        public string Pantheon { get; set; }
        public string Pros { get; set; }
        public string Roles { get; set; }
        public int Speed { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string abilityDescription1 { get; set; }
        public string abilityDescription2 { get; set; }
        public string abilityDescription3 { get; set; }
        public string abilityDescription4 { get; set; }
        public string abilityDescription5 { get; set; }
        public string latestChampion { get; set; }
    }
}
