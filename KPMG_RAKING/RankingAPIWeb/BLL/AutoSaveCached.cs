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
        const string ListCacheLastUpdate = "https://localhost:44351/api/GameResult/lastUpdated";
        const string intervalRefreshLink = "https://localhost:44351/api/GameResult/intervalRefresh";
        HttpClient httpClient = new HttpClient();
        private DaoGameResult daoGameResult;

        int intervalRefresh
        {
            get
            {
                //se não for definido um intervalo para as atualizações, por padrão será atualizado a cada 1 minuto
                var interval = GetInvervalRefresh();
                 return interval.Result * 1000 * 60;
            }
        }

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
            Timer();
        }

        private async Task Timer()
        {
            while (true)
            {
                await SaveCachedAsync();
                await Task.Delay(intervalRefresh);
            }
        }


        public async Task SaveCachedAsync()
        {
            try
            {
                var lastUpdated = await GetLastUpdated();
                var intervalRefresh = await GetInvervalRefresh();
                var timeNow = DateTime.Now;
                TimeSpan differenteTime = timeNow - lastUpdated;
                if (differenteTime.Minutes > intervalRefresh)
                {
                    var cachedList = await GetCachedData();
                    if (cachedList != null && cachedList.Count > 0)
                    {
                        foreach (var item in cachedList)
                        {
                            await daoGame.SaveAllGamesScore(item);
                        }
                        _ =  ClearCachedData();
                        await SetLastUpdated(DateTime.Now);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<Model.GameResult>> GetCachedData()
        {
            try
            {
                var response = await httpClient.GetAsync(cachedLink);
                var responseData = await response.Content.ReadAsStringAsync();
                var ListOfCachedData = JsonConvert.DeserializeObject<List<Model.GameResult>>(responseData);
                return ListOfCachedData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<DateTime> GetLastUpdated()
        {
            try
            {
                var response = await httpClient.GetAsync(ListCacheLastUpdate);
                var responseData = await response.Content.ReadAsStringAsync();
                if (responseData == "") return DateTime.MinValue;
                else
                {
                    var LastUpdated = JsonConvert.DeserializeObject<DateTime>(responseData);
                    return LastUpdated;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SetLastUpdated(DateTime lastUpdated)
        {
            try
            {
                var json = JsonConvert.SerializeObject(lastUpdated, Formatting.Indented);
                var stringContent = new StringContent(json);
                await httpClient.PostAsync(ListCacheLastUpdate, stringContent);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<int> GetInvervalRefresh()
        {
            try
            {
                var response = await httpClient.GetAsync(intervalRefreshLink);
                var responseData = await response.Content.ReadAsStringAsync();
                    var interval = JsonConvert.DeserializeObject<int>(responseData);
                    return interval;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task ClearCachedData()
        {
            try
            {
                bool value = true;
                var json = JsonConvert.SerializeObject(value, Formatting.Indented);
                var stringContent = new StringContent(json);
                await httpClient.PostAsync(cachedLink, stringContent);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
