﻿using Microsoft.AspNetCore.Http;
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
        public IMemoryCache _memoryCache;

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
            //CheckToSaveCachedData();
            return Ok();
        }

        [HttpGet]
        [Route("cached")]
        public ActionResult GetCachedResults()
        {
            var result = _memoryCache.Get("Resultados");
            return Ok(result);
        }

        [HttpPost]
        [Route("cached")]
        public ActionResult ClearCachedData()
        {
            _memoryCache.Remove("Resultados");
            return Ok();
        }

        [HttpGet]
        [Route("intervalRefresh")]
        public ActionResult GetIntervalRefresh()
        {
            return Ok(_memoryCache.Get("intervalRefresh"));
        }

        [HttpPost]
        [Route("intervalRefresh")]
        public ActionResult SetInvervalRefresh(int minutes)
        {
            _memoryCache.Set("intervalRefresh", minutes);
            return Ok(minutes);
        }

        [HttpGet]
        [Route("lastUpdated")]
        public IActionResult GetLastUpdate()
        {
            return Ok(_memoryCache.Get("LastUpdate"));
        }

        [HttpPost]
        [Route("lastUpdated")]
        public IActionResult SetLastUpdate(DateTime LastUpdate)
        {
            return Ok(_memoryCache.Set("LastUpdate", LastUpdate));
        }

    }
}
