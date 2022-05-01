using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RankingAPIWeb.DAO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace RankingAPIWeb.BLL
{
    public class AutoSaveCached
    {
        const string cachedLink = "https://localhost:44351/api/GameResult/cached";
        const string ListCacheLastUpdate = "https://localhost:44351/api/GameResult/lastUpdated";
        const string intervalRefreshLink = "https://localhost:44351/api/GameResult/intervalRefresh";
        HttpClient httpClient = new HttpClient();
        private DaoGameResult daoGameResult;
        private Controllers.GameResultController gameResultController = new Controllers.GameResultController();


        int intervalRefresh
        {
            get
            {
                //se não for definido um intervalo para as atualizações, por padrão será atualizado a cada 1 minuto
                var result = gameResultController.GetIntervalRefresh().Result as OkObjectResult;
                var interval = (int)result.Value;
                return interval;
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
                await Task.Delay(intervalRefresh * 1000 * 60);
            }
        }


        public async Task SaveCachedAsync()
        {
            try
            {
                var lastUpdated = await GetLastUpdate();
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
                        gameResultController.ClearCachedData();
                        gameResultController.SetLastUpdate(timeNow);
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
                var result = gameResultController.GetCachedResults().Result as OkObjectResult;
                return (List<Model.GameResult>)result.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<DateTime> GetLastUpdate()
        {
            try
            {
                var result = gameResultController.GetLastUpdate().Result as OkObjectResult;
                return (DateTime)result.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
