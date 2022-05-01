using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RankingAPIWeb.DAO;

namespace RankingAPIWeb.Controllers
{
    [ApiController]
    [Route("api/GameResult")]
    [Produces("application/json")]
    public class GameResultController : ControllerBase
    {
        private DaoGameResult daoGameResult;

        public static List<Model.GameResult> GameResults = new List<Model.GameResult>();
        public static int IntervalRefresh = 1;
        public static DateTime LastUpdate = DateTime.MinValue;

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
            try
            {
                GameResults.Add(gameResult);
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
        public ActionResult<List<Model.GameResult>> GetCachedResults()
        {
            try
            {
                return Ok(GameResults);
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
                GameResults.Clear();
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
                if (IntervalRefresh == 0) return Ok(1999);
                else return Ok(IntervalRefresh);
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
                IntervalRefresh = minutes;
                return Ok(minutes);
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

        [HttpGet]
        [Route("lastUpdated")]
        public ActionResult<DateTime> GetLastUpdate()
        {
            try
            {
                return Ok(LastUpdate);
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

        [HttpPost]
        [Route("lastUpdated")]
        public ActionResult SetLastUpdate(DateTime lastDate)
        {
            try
            {
                LastUpdate = lastDate;
                return Ok(lastDate);
            }
            catch (Exception ex)
            {

                return BadRequest("Ocorreu um erro inesperado:" + ex.Message);
            }
        }

    }
}
