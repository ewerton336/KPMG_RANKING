using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiteRankingKPMG.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RankingAPIWeb.BLL;
using RankingAPIWeb.Model;
namespace SiteRankingKPMG.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        const string rankingLink = "https://ewertondev.com.br/api/GameResult/";
        const string lastUpdateLink = "https://ewertondev.com.br/api/GameResult/lastUpdated";
        private HttpClient client = new HttpClient();
        private AutoSaveCached autoSave = new AutoSaveCached();
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            //pela aplicação em produção estar rodando em single thread, a task de verificar e salvar periodicamente não funciona em golive
            //por isso coloquei este método abaixo para fazer a verificação e possivel salvaemnto toda vez que abrir a página index.
            _ = autoSave.SaveCachedAsync();

            //obter ranking
            var get = client.GetAsync(rankingLink).Result;
            var result = get.Content.ReadAsStringAsync().Result;
            var ListRanking = JsonConvert.DeserializeObject<List<GameResult>>(result);
            ViewBag.Ranking = ListRanking;

            var getLastUpdate = client.GetAsync(lastUpdateLink).Result;
            var resultLastUpdate = getLastUpdate.Content.ReadAsStringAsync().Result;
            var LastUpdate = JsonConvert.DeserializeObject<DateTime>(resultLastUpdate);
            ViewBag.LastUpdate = LastUpdate;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
