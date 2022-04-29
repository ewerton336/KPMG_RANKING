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
        public IMemoryCache _memoryCache;
        private List<Model.GameResult> games = new List<Model.GameResult>();


        public static List<Model.GameResult> Cats = new List<Model.GameResult>();
   

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


        /// <summary>
        /// Obtém a lista dos top 100 jogadores e suas pontuações
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// GET api/GameResult
        /// </remarks>
        /// <returns>Uma lista com os 100 jogadores com maior pontuação, em ordem decrescente</returns>
        /// /// <response code="200">Uma lista com os 100 jogadores com maior pontuação, em ordem decrescente.</response>
        /// <response code="500">Ocorreu algo inesperado. Tente novamente mais tarde.</response>
        [HttpGet]
        // GET: GameResultController
        public ActionResult Index()
        {
            try
            {
                var teste2 = daoGame.GetTop100Players().Result;
                return Ok(teste2);
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }



        /// <summary>
        /// Envia a pontuação de uma partida para API
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// {
        /// "playerId": 0,
        /// "gameScore": 0,
        /// "gameDate": "2022-04-29T15:34:23.859Z"
        /// }
        /// GET api/GameResult
        /// </remarks>
        /// <response code="200">Uma lista com os 100 jogadores com maior pontuação, em ordem decrescente.</response>
        /// <response code="500">Ocorreu algo inesperado. Tente novamente mais tarde.</response>
        [HttpPost]
        // POST: GameResultController
        public ActionResult SaveDataGameScore(Model.GameResult gameResult)
        {
            Cats.Add(gameResult);
            try
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
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

        /// <summary>
        /// Obtém a lista de pontuações salvas em cachê
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// GET api/GameResult/cached
        /// </remarks>
        /// <returns>Uma lista com as pontuações em cachê</returns>
        /// /// <response code="200">Uma lista com os 100 jogadores com maior pontuação, em ordem decrescente.</response>
        /// <response code="500">Ocorreu algo inesperado. Tente novamente mais tarde.</response>
        [HttpGet]
        [Route("cached")]
        public ActionResult GetCachedResults()
        {
        
            try
            {
                var result = _memoryCache.Get("Resultados");
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

        /// <summary>
        /// Limpa os dados em cachÊ
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// POST api/GameResult/cached
        /// </remarks>
        /// /// <response code="200">Uma lista com os 100 jogadores com maior pontuação, em ordem decrescente.</response>
        /// <response code="500">Ocorreu algo inesperado. Tente novamente mais tarde.</response>
        [HttpPost]
        [Route("cached")]
        public ActionResult ClearCachedData()
        {
            try
            {
                _memoryCache.Remove("Resultados");
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message); ;
            }
        }

        [HttpGet]
        [Route("intervalRefresh")]
        public ActionResult<int> GetIntervalRefresh()
        {
            try
            {
                var interval = _memoryCache.Get("intervalRefresh");
                if (interval == null) return Ok(1);
                else return Ok(_memoryCache.Get("intervalRefresh"));
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

        [HttpPost]
        [Route("intervalRefresh")]
        public ActionResult SetInvervalRefresh(int minutes)
        {
            try
            {
                _memoryCache.Set("intervalRefresh", minutes);
                return Ok(minutes);
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

        [HttpGet]
        [Route("lastUpdated")]
        public IActionResult GetLastUpdate()
        {
            try
            {
                return Ok(_memoryCache.Get("LastUpdate"));
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

        [HttpPost]
        [Route("lastUpdated")]
        public IActionResult SetLastUpdate(DateTime LastUpdate)
        {
            try
            {
                return Ok(_memoryCache.Set("LastUpdate", LastUpdate));
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

    }
}
