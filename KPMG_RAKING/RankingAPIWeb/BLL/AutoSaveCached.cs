using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RankingAPIWeb.DAO;

namespace RankingAPIWeb.BLL
{
    public class AutoSaveCached
    {
        const string cachedLink = "https://localhost:44351/api/GameResult/cached";
        const string cacheLastUpdate = "https://localhost:44351/api/GameResult/lastUpdated";
        HttpClient httpClient = new HttpClient();
        private DaoGameResult daoGameResult;

        DaoGameResult daoGame
        {
            get
            {
                if (daoGameResult == null)
                {
                    daoGameResult = new DaoGameResult(Helper.DBConnectionMySql);
                }
                return daoGameResult;
            }
            set
            {
                daoGameResult = value;
            }
        }

        public AutoSaveCached()
        {
            _ = Timer();
        }

        private async Task Timer()
        {
            while (true)
            {
                _ = SaveCachedAsync();
                await Task.Delay(5000);
            }
        }


        private async Task SaveCachedAsync()
        {
            var lastUpdated = await GetLastUpdated();
            var timeNow = DateTime.Now;
            TimeSpan differenteTime = timeNow - lastUpdated;
            if (differenteTime.Minutes > 5)
            {
                var cachedList = await GetCachedData();
                if (cachedList != null && cachedList.Count > 0)
                {
                    foreach (var item in cachedList)
                    {
                        await daoGame.SaveAllGamesScore(item);
                    }
                    await SetLastUpdated(DateTime.Now);
                }
            }

        }

        private async Task<List<Model.GameResult>> GetCachedData()
        {
            var response = await httpClient.GetAsync(cachedLink);
            var responseData = await response.Content.ReadAsStringAsync();
            var ListOfCachedData = JsonConvert.DeserializeObject<List<Model.GameResult>>(responseData);
            return ListOfCachedData;
        }

        private async Task<DateTime> GetLastUpdated()
        {
            var response = await httpClient.GetAsync(cacheLastUpdate);
            var responseData = await response.Content.ReadAsStringAsync();
            var LastUpdated = JsonConvert.DeserializeObject<DateTime>(responseData);
            return LastUpdated;
        }

        private async Task SetLastUpdated(DateTime lastUpdated)
        {
            var json = JsonConvert.SerializeObject(DateTime.Now, Formatting.Indented);
            var stringContent = new StringContent(json);
            await httpClient.PostAsync(cacheLastUpdate, stringContent);
        }
    }
}
