using Newtonsoft.Json;
using PaladinsAPI.Enumerations;
using PaladinsAPI.Models;
using PaladinsAPI.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PaladinsAPI.Exceptions;

namespace PaladinsAPI
{
    public class PaladinsApi
    {
        private readonly string _devId;
        private readonly string _authKey;
        private readonly string _paladinsApiUrl;

        private Session _currentSession;
        private DateTime _lastSession;

        private string _timestamp;

        public PaladinsApi(string devId, string authKey, Platform platform)
        {
            _devId = devId;
            _authKey = authKey;

            switch (platform)
            {
                case Platform.Pc:
                    _paladinsApiUrl = "http://api.paladins.com/paladinsapi.svc/";
                    break;
                case Platform.Xbox:
                    _paladinsApiUrl = "http://api.xbox.paladins.com/paladinsapi.svc/";
                    break;
                case Platform.Ps4:
                    _paladinsApiUrl = "http://api.ps4.paladins.com/paladinsapi.svc/";
                    break;
                default:
                    _paladinsApiUrl = "http://api.paladins.com/paladinsapi.svc/";
                    break;
            }
        }

        public async Task<PatchInfo> GetPatchInfo()
        {
            return await Request<PatchInfo>("getpatchinfo");
        }

        public async Task<DataUsed> GetDataUsed()
        {
            return (await Request<List<DataUsed>>("getdataused")).ElementAt(0);
        }
        public async Task<List<MatchDetails>> GetMotd()
        {
            return await Request<List<MatchDetails>>("getmotd");
        }
        public async Task<Player> GetPlayer(string name)
        {
            return (await Request<List<Player>>("getplayer", name)).ElementAt(0);
        }
        public async Task<PlayerAchievements> GetPlayerAchievements(int id)
        {
            return (await Request<PlayerAchievements>("getplayerachievements", id));
        }
        public async Task<PlayerStatus> GetPlayerStatus(string playerName)
        {
            return (await Request<List<PlayerStatus>>("getplayerstatus", playerName)).ElementAt(0);
        }
        public async Task<List<PlayerLoadouts>> GetPlayerLoadouts(int playerId)
        {
            return await Request<List<PlayerLoadouts>>("getplayerloadouts", playerId);
        }
        public async Task<List<Friend>> GetFriends(string playerName)
        {
            return await Request<List<Friend>>("getfriends", playerName);
        }
        public async Task<List<Friend>> GetFriends(int playerId)
        {
            return await Request<List<Friend>>("getfriends", playerId);
        }
        public async Task<List<TopMatch>> GetTopMatches()
        {
            return await Request<List<TopMatch>>("gettopmatches");
        }
        public async Task<List<MatchId>> GetMatchIdsByQueueAsync(QueueType queue, string date, string hour)
        {
            return await Request<List<MatchId>>("getmatchidsbyqueue", queue, date, hour);
        }
        public async Task<List<MatchPlayer>> GetMatchPlayer(int matchId)
        {
            return await Request<List<MatchPlayer>>("getmatchplayerdetails", matchId);
        }
        public async Task<List<MatchDetails>> GetMatchDetails(int matchId)
        {
            return await Request<List<MatchDetails>>("getmatchdetails", matchId);
        }
        public async Task<List<MatchHistory>> GetMatchHistory(string playerName)
        {
            return await Request<List<MatchHistory>>("getmatchhistory", playerName);
        }
        public async Task<List<MatchHistory>> GetMatchHistory(int playerId)
        {
            return await Request<List<MatchHistory>>("getmatchhistory", playerId);
        }
        public async Task<List<Item>> GetItems(eLanguageCode languageCode)
        {
            return await Request<List<Item>>("getitems", languageCode);
        }
        public async Task<List<Champion>> GetChampions(eLanguageCode languageCode)
        {
            return await Request<List<Champion>>("getchampions", languageCode);
        }
        public async Task<List<ChampionSkin>> GetChampionSkins(int championId, eLanguageCode languageCode)
        {
            return await Request<List<ChampionSkin>>("getchampionskins", championId, languageCode);
        }
        public async Task<List<ChampionRank>> GetChampionRanks(string playerName)
        {
            return await Request<List<ChampionRank>>("getchampionranks", playerName);
        }
        public async Task<List<ChampionRank>> GetChampionRanks(int playerId)
        {
            return await Request<List<ChampionRank>>("getchampionranks", playerId);
        }
        public async Task<List<QueueChampionStat>> GetQueueStats(string playerName, QueueType queue)
        {
            return await Request<List<QueueChampionStat>>("getqueuestats", playerName, queue);
        }
        public async Task<List<QueueChampionStat>> GetQueueStats(int playerId, QueueType queue)
        {
            return await Request<List<QueueChampionStat>>("getqueuestats", playerId, queue);
        }

        public async Task<T> Request<T>(string apiMethod, params dynamic[] parameters)
        {
            /* Check if for any reason session is not established */
            if(string.IsNullOrEmpty(_currentSession?.session_id))
                await CreateSession();

            /* Check if session is dead */
            if (DateTime.UtcNow.Subtract(_lastSession).TotalMinutes >= 15)
                await CreateSession();

            /* Creating the signature from devID, apiMethod, authKey and _timestamp*/
            var signature = SecurityUtilities.GetMD5Hash(_devId + apiMethod + _authKey + _timestamp);

            /* Generating the url we going to HTTP GET */
            var url = _paladinsApiUrl + apiMethod + "json" + "/" + _devId + "/" + signature + "/" + _currentSession.session_id + "/" + _timestamp;

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
            var client = new HttpClient();
            var jsonResponse = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<T>(jsonResponse);

            /* Variables to hold the exception if any */
            var foundProblem = false;
            var errorMessage = string.Empty;

            /* Player Achievements and Patch Info is the only one that does not contain list,
             * so there is no generictypedefinition and we will get an
             * exception */
            if (result is PlayerAchievements || result is PatchInfo)
            {
                var basic = result as PaladinsResponse;
                if (basic.ret_msg != null)
                {
                    foundProblem = true;
                    errorMessage = basic.ret_msg;
                }
            }
            /* If T is a List, then get the first element */
            else if (typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = (IList)result;
                if (list != null && list.Count > 0)
                {
                    var mine = list[0] as PaladinsResponse;
                    if (mine?.ret_msg != null) foundProblem = true;
                }
            }
            else
            {
                var basic = result as PaladinsResponse;
                if (basic?.ret_msg != null)
                {
                    foundProblem = true;
                    errorMessage = basic.ret_msg;
                }
            }

            /* If there is no problem in the response,
             * return the values */
            if (!foundProblem) return result;

            /* If there is a problem in the response, try again.
             * It is only check for dead session 
             * the recursion. */
            if (errorMessage.Contains("dailylimit"))
            {
                throw new DailyLimitException();
            }
            if (errorMessage.Contains("Exception while validating developer access"))
            {
                throw new WrongCredentialsException();
            }
            if (errorMessage.Contains("404"))
            {
                throw new NotFoundException();
            }

            await CreateSession();
            return await Request<T>(apiMethod, parameters);
        }

        public async Task CreateSession()
        {
            /* Format the _timestamp */
            _timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            /* Update the _lastSession */
            _lastSession = DateTime.UtcNow;

            /* Creating the signature from devID, createsession(), authKey and _timestamp*/
            var signature = SecurityUtilities.GetMD5Hash(_devId + "createsession" + _authKey + _timestamp);
            /* Generating the url we going to HTTP GET */
            var url = _paladinsApiUrl + "createsession" + "json" + "/" + _devId + "/" + signature + "/" + _timestamp;

            /* Intialize the httpclient, HTTP GET the url and deserialize the response */
            var client = new HttpClient();
            var jsonResponse = await client.GetStringAsync(url);
            _currentSession = JsonConvert.DeserializeObject<Session>(jsonResponse);

            /* If session cannot be established, throw an exception */
            if (string.IsNullOrEmpty(_currentSession?.session_id))
            {
                throw new Exception("Session could not be created!");
            }
        }
    }
}