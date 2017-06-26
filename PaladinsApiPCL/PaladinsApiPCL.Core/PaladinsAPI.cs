using Newtonsoft.Json;
using PaladinsApiPCL.Core.Domain.Enumerations;
using PaladinsApiPCL.Core.Domain.Models;
using PaladinsApiPCL.Core.Domain.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaladinsApiPCL.Core
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
        public static async Task<List<PlayerLoadouts>> GetPlayerLoadouts(int playerId)
        {
            return await Request<List<PlayerLoadouts>>("getplayerloadouts", playerId);
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
            string signature = SecurityUtilities.GetMD5Hash(devId + apiMethod + authKey + timestamp);

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
            if (result is PlayerAchievements)
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
            string signature = SecurityUtilities.GetMD5Hash(devId + "createsession" + authKey + timestamp);
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
    }
}