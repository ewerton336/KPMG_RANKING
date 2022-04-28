using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RankingAPIWeb.DAO;
using Microsoft.Extensions.Caching.Memory;

namespace RankingAPIWeb.Controllers
{
    [ApiController]
    [Route("api/GameResult")]
    [Produces("application/json")]
    public class GameResultController : ControllerBase
    {
        private DaoGameResult daoGameResult;
        private readonly IMemoryCache _memoryCache;
        public GameResultController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }



        //método para instanciar uma DAO somente uma vez, para assim não abrir várias conexões com o banco de dados
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


        [HttpGet]
        // GET: GameResultController
        public ActionResult Index()
        {
            var teste2 = daoGame.GetTop100Players().Result;
            return Ok(teste2);
        }


        [HttpPost]
        // POST: GameResultController
        public ActionResult SaveDataGameScore(Model.GameResult gameResult)
        {
            var ResultList = (List<Model.GameResult>)_memoryCache.Get("Resultados");
            if (ResultList != null && ResultList.Count > 0)
            {
                ResultList.Add(gameResult);
                _memoryCache.Set("Resultados", ResultList);
            }
            else
            {
                List<Model.GameResult> list = new List<Model.GameResult>();
                list.Add(gameResult);
                _memoryCache.Set("Resultados", list);
            }
            CheckToSaveCachedData();
            return Ok();
        }

        [HttpGet]
        [Route("cached")]
        public ActionResult GetCachedResults()
        {
            return Ok(_memoryCache.Get("Resultados"));
        }


        private async Task CheckToSaveCachedData()
        {
            _memoryCache.Set("IntervalTime", 1);
            int interval = (int)_memoryCache.Get("IntervalTime");

            var lastUpdate = _memoryCache.Get("LastUpdate");
            var timeNow = DateTime.Now;
            TimeSpan differenteTime = lastUpdate != null ? timeNow -(DateTime)lastUpdate   : timeNow - DateTime.MinValue ;
            if (differenteTime.Minutes > interval)
            {
                var cachedList = (List<Model.GameResult>)_memoryCache.Get("Resultados");
                if (cachedList != null && cachedList.Count > 0)
                {
                    foreach (var item in cachedList)
                    {
                        await daoGame.SaveAllGamesScore(item);
                    }
                    _memoryCache.Set("LastUpdate", DateTime.Now);
                }
            }
        }
    }
}
