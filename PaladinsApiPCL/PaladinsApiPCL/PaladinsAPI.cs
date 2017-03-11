using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PaladinsAPI.Models
{
    public static class PaladinsAPI
    {
        public static string devId;
        public static string authKey;
        public static DateTime lastSession;
        public static Session currentSession;
        public static bool goingToCreateSession;
        public static string timestamp;

        public static string PaladinsApiUrl = "http://api.paladins.com/paladinsapi.svc/";

        public enum eLanguageCode
        {
            English = 1,
            German = 2,
            French = 3,
            Spanish = 7,
            Spanish_Latin_America = 9,
            Portuguese = 10,
            Russian = 11, 
            Polish = 12,
            Turkish = 13
        }

        public enum QueueType
        {
            Challenge_K_StoneKeep = 423,
            Casual = 424,
            PracticeVSAI = 425,
            ChallengeMatch = 426,
            Practice = 427,
            SoloCompetitive = 428,
            zzRETIRED = 429,
            Challenge_F_TimberMI = 430,
            Challenge_F_Dock = 431,
            Challenge_I_Igloo = 432,
            Challenge_T_Isle = 433,
            ShootingRange = 434,
            PerfCaptureMap = 435,
            TencentAlphaTestQueueCoop = 436,
            Payload = 437,
            Challenge_T_Temple = 438,
            Challenge_I_Mine = 439,
            Challenge_T_Beach = 440,
            Challenge_TP = 441,
            Challenge_FP = 442,
            Challenge_IP = 443,
            Tutorial = 444,
            LiveTestMaps = 445,
            PvEHandsThatBind = 446,
            WIPPvELosPollosFernandos = 447,
            WIPPvEHighRollers = 448,
            PvEHnS = 449,
            WIPPvELeapFrogs = 450,
            PvESurvival = 451,
            LIVESurvival = 452,
            LIVESurvivalPractice = 453,
            Challenge_I_SurvivalRitual = 454,
            Challenge_T_SurvivalArena = 455,
            Multi_Queue = 999
        }

        public static void Initializer(string devId, string authKey)
        {
            PaladinsAPI.devId = devId;
            PaladinsAPI.authKey = authKey;
        }

        public static async Task<DataUsed> GetDataUsed()
        {
            return (await Request<List<DataUsed>>("getdataused")).ElementAt(0);
        }

        public static async Task<List<MatchDetails>> GetMOTD()
        {
            return await Request<List<MatchDetails>>("getmotd");
        }

        public static async Task<Player> GetPlayer(string name)
        {
            return (await Request<List<Player>>("getplayer", name)).ElementAt(0);
        }

        public static async Task<PlayerAchievements> GetPlayerAchievements(int id)
        {
            return (await Request<PlayerAchievements>("getplayerachievements", id));
        }

        public static async Task<PlayerStatus> GetPlayerStatus(string playerName)
        {
            return (await Request<List<PlayerStatus>>("getplayerstatus", playerName)).ElementAt(0);
        }

        public static async Task<List<Friend>> GetFriends(string playerName)
        {
            return await Request<List<Friend>>("getfriends", playerName);
        }

        public static async Task<List<Friend>> GetFriends(int playerId)
        {
            return await Request<List<Friend>>("getfriends", playerId);
        }

        public static async Task<List<TopMatch>> GetTopMatches()
        {
            return await Request<List<TopMatch>>("gettopmatches");
        }

        public static async Task<List<MatchId>> GetMatchIdsByQueueAsync(QueueType queue, string date, string hour)
        {
            return await Request<List<MatchId>>("getmatchidsbyqueue", queue, date, hour);
        }

        public static async Task<List<MatchPlayer>> GetMatchPlayer(int matchId)
        {
            return await Request<List<MatchPlayer>>("getmatchplayerdetails", matchId);
        }

        public static async Task<List<MatchDetails>> GetMatchDetails(int matchId)
        {
            return await Request<List<MatchDetails>>("getmatchdetails", matchId);
        }

        public static async Task<List<MatchHistory>> GetMatchHistory(string playerName)
        {
            return await Request<List<MatchHistory>>("getmatchhistory", playerName);
        }

        public static async Task<List<MatchHistory>> GetMatchHistory(int playerId)
        {
            return await Request<List<MatchHistory>>("getmatchhistory", playerId);
        }

        public static async Task<List<Item>> GetItems(eLanguageCode languageCode)
        {
            return await Request<List<Item>>("getitems", languageCode);
        }

        public static async Task<List<Champion>> GetChampions(eLanguageCode languageCode)
        {
            return await Request<List<Champion>>("getchampions", languageCode);
        }

        public static async Task<List<ChampionSkin>> GetChampionSkins(int championId, eLanguageCode languageCode)
        {
            return await Request<List<ChampionSkin>>("getchampionskins", championId, languageCode);
        }

        public static async Task<List<ChampionRank>> GetChampionRanks(string playerName)
        {
            return await Request<List<ChampionRank>>("getchampionranks", playerName);
        }

        public static async Task<List<ChampionRank>> GetChampionRanks(int playerID)
        {
            return await Request<List<ChampionRank>>("getchampionranks", playerID);
        }

        public static async Task<List<QueueChampionStat>> GetQueueStats(string playerName, QueueType queue)
        {
            return await Request<List<QueueChampionStat>>("getqueuestats", playerName, queue);
        }

        public static async Task<List<QueueChampionStat>> GetQueueStats(int playerId, QueueType queue)
        {
            return await Request<List<QueueChampionStat>>("getqueuestats", playerId, queue);
        }

        public static async Task<T> Request<T>(string apiMethod, params dynamic[] parameters)
        {
            /* Check if for any reason session is not established */
            if (currentSession == null
                || currentSession.session_id == null
                || currentSession.session_id == "")
                await CreateSession();

            /* Check if session is dead */
            if (DateTime.UtcNow.Subtract(lastSession).TotalMinutes >= 15)
                await CreateSession();

            /* Creating the signature from devID, apiMethod, authKey and timestamp*/
            string signature = GetMD5Hash(devId + apiMethod + authKey + timestamp);

            /* Generating the url we going to HTTP GET */
            string url = PaladinsApiUrl + apiMethod + "json" + "/" + devId + "/" + signature + "/" + currentSession.session_id + "/" + timestamp;

            /* Add every parameter we pushed into the request */
            foreach (var param in parameters)
            {
                string stringParam = "/";

                /* If the parameter is DateTime the format must be yyyyMMdd */
                if (param is DateTime) stringParam += param.ToString("yyyyMMdd");
                /* If its enum, cast to int and then in string */
                else if (param is QueueType || param is eLanguageCode) stringParam += ((int)param).ToString();
                /* If it is every other just convert into string */
                else stringParam += param.ToString();

                /* Add it to url we going to get */
                url += stringParam;
            }

            /* Intialize the httpclient, HTTP GET the url and deserialize the response */
            HttpClient client = new HttpClient();
            var JsonResponse = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<T>(JsonResponse);

            bool found_problem = false;
            /* Player Achievements is the only one that does not contain list,
             * so there is no generictypedefinition and we will get an
             * exception */
            if(result is PlayerAchievements)
            {
                PaladinsResponse basic = result as PaladinsResponse;
                if (basic.ret_msg != null) found_problem = true;
            }
            /* If T is a List, then get the first element */
            else if (typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                IList list = result as IList;
                if (list.Count > 0)
                {
                    PaladinsResponse mine = list[0] as PaladinsResponse;
                    if (mine.ret_msg != null) found_problem = true;
                }
            }
            else
            {
                PaladinsResponse basic = result as PaladinsResponse;
                if (basic.ret_msg != null) found_problem = true;
            }

            /* If there is a problem in the response, try again.
             * It is only check for dead session 
             * TODO check for maxsessions reached etc so to stop
             * the recursion. */
            if (found_problem)
            {
                await CreateSession();
                return await Request<T>(apiMethod, parameters);
            }
            else return result;
        }

        public static async Task CreateSession()
        {
            /* Format the timestamp */
            timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            /* Update the lastSession */
            lastSession = DateTime.UtcNow;

            /* Creating the signature from devID, createsession(), authKey and timestamp*/
            string signature = GetMD5Hash(devId + "createsession" + authKey + timestamp);
            /* Generating the url we going to HTTP GET */
            string url = PaladinsApiUrl + "createsession" + "json" + "/" + devId + "/" + signature + "/" + timestamp;

            /* Intialize the httpclient, HTTP GET the url and deserialize the response */
            HttpClient client = new HttpClient();
            var JsonResponse = await client.GetStringAsync(url);
            currentSession = JsonConvert.DeserializeObject<Session>(JsonResponse);

            /* If session cannot be established, throw an exception */
            if (currentSession == null
                || currentSession.session_id == null
                || currentSession.session_id == "") throw new Exception("Session could not be created!");
        }

        public static string GetMD5Hash(string input)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = md5.ComputeHash(bytes);
            var sb = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }

    /*
     * We use PaladinsResponse as the base class to upcast  
     * all children and check for errors
     */
    public class PaladinsResponse
    {
        public string ret_msg { get; set; }
    }
    public class Session : PaladinsResponse
    {
        public string session_id { get; set; }
        public string timestamp { get; set; }
    }
    public class DataUsed : PaladinsResponse
    {
        public int Active_Sessions { get; set; }
        public int Concurrent_Sessions { get; set; }
        public int Request_Limit_Daily { get; set; }
        public int Session_Cap { get; set; }
        public int Session_Time_Limit { get; set; }
        public int Total_Requests_Today { get; set; }
        public int Total_Sessions_Today { get; set; }
    }
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
    public class PlayerAchievements : PaladinsResponse
    {
        public int AssistedKills { get; set; }
        public int CampsCleared { get; set; }
        public int DivineSpree { get; set; }
        public int DoubleKills { get; set; }
        public int FireGiantKills { get; set; }
        public int FirstBloods { get; set; }
        public int GodLikeSpree { get; set; }
        public int GoldFuryKills { get; set; }
        public int Id { get; set; }
        public int ImmortalSpree { get; set; }
        public int KillingSpree { get; set; }
        public int MinionKills { get; set; }
        public string Name { get; set; }
        public int PentaKills { get; set; }
        public int PhoenixKills { get; set; }
        public int PlayerKills { get; set; }
        public int QuadraKills { get; set; }
        public int RampageSpree { get; set; }
        public int ShutdownSpree { get; set; }
        public int SiegeJuggernautKills { get; set; }
        public int TowerKills { get; set; }
        public int TripleKills { get; set; }
        public int UnstoppableSpree { get; set; }
        public int WildJuggernautKills { get; set; }
    }
    public class PlayerStatus : PaladinsResponse
    {
        public int Match { get; set; }
        public string personal_status_message { get; set; }
        public int status { get; set; }
        public string status_string { get; set; }
        public int playerId { get; set; }
    }
    public class Friend : PaladinsResponse
    {
        public string account_id { get; set; }
        public string name { get; set; }
        public string player_id { get; set; }
    }
    public class ChampionRank : PaladinsResponse
    {
        public int Id { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        public int Kills { get; set; }
        public int Losses { get; set; }
        public int MinionKills { get; set; }
        public int Rank { get; set; }
        public int Wins { get; set; }
        public int Worshippers { get; set; }
        public string champion { get; set; }
        public string champion_id { get; set; }
        public string player_id { get; set; }
    }
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
    public class ChampionSkin : PaladinsResponse
    {
        public int champion_id { get; set; }
        public string champion_name { get; set; }
        public int skin_id1 { get; set; }
        public int skin_id2 { get; set; }
        public string skin_name { get; set; }
    }

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
    public class MatchId : PaladinsResponse
    {
        public string Active_Flag { get; set; }
        public int Match { get; set; }
    }
    public class MatchDetails : PaladinsResponse
    {
        public int Account_Level { get; set; }
        public int ActiveId1 { get; set; }
        public int ActiveId2 { get; set; }
        public int Assists { get; set; }
        public string Ban1 { get; set; }
        public int Ban1Id { get; set; }
        public string Ban2 { get; set; }
        public int Ban2Id { get; set; }
        public string Ban3 { get; set; }
        public int Ban3Id { get; set; }
        public string Ban4 { get; set; }
        public int Ban4Id { get; set; }
        public int Damage_Bot { get; set; }
        public int Damage_Done_Magical { get; set; }
        public int Damage_Done_Physical { get; set; }
        public int Damage_Mitigated { get; set; }
        public int Damage_Player { get; set; }
        public int Damage_Taken { get; set; }
        public int Deaths { get; set; }
        public string Entry_Datetime { get; set; }
        public int Final_Match_Level { get; set; }
        public int GodId { get; set; }
        public int Gold_Earned { get; set; }
        public int Gold_Per_Minute { get; set; }
        public int Healing { get; set; }
        public int ItemId1 { get; set; }
        public int ItemId2 { get; set; }
        public int ItemId3 { get; set; }
        public int ItemId4 { get; set; }
        public int ItemId5 { get; set; }
        public int ItemId6 { get; set; }
        public string Item_Active_1 { get; set; }
        public string Item_Active_2 { get; set; }
        public string Item_Active_3 { get; set; }
        public string Item_Purch_1 { get; set; }
        public string Item_Purch_2 { get; set; }
        public string Item_Purch_3 { get; set; }
        public string Item_Purch_4 { get; set; }
        public string Item_Purch_5 { get; set; }
        public string Item_Purch_6 { get; set; }
        public int Killing_Spree { get; set; }
        public int Kills_Bot { get; set; }
        public int Kills_Player { get; set; }
        public int Mastery_Level { get; set; }
        public int Match { get; set; }
        public int Minutes { get; set; }
        public int Multi_kill_Max { get; set; }
        public int PartyId { get; set; }
        public string Reference_Name { get; set; }
        public string Skin { get; set; }
        public int SkinId { get; set; }
        public int Structure_Damage { get; set; }
        public string Surrendered { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int TeamId { get; set; }
        public string Team_Name { get; set; }
        public string Win_Status { get; set; }
        public string name { get; set; }
        public string playerName { get; set; }
    }
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
    public class MatchHistory : PaladinsResponse {
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
    public class Item : PaladinsResponse
    {
        public string Description { get; set; }
        public string DeviceName { get; set; }
        public int IconId { get; set; }
        public int ItemId { get; set; }
        public int Price { get; set; }
        public string ShortDesc { get; set; }
    }
    public class GameInfo : PaladinsResponse
    {
        public int Leaves { get; set; }
        public int Losses { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int PrevRank { get; set; }
        public int Rank { get; set; }
        public int Season { get; set; }
        public int Tier { get; set; }
        public int Trend { get; set; }
        public int VictoryPoints { get; set; }
        public int Wins { get; set; }
    }
}