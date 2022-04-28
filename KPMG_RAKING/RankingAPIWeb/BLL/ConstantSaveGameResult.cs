using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RankingAPIWeb.BLL
{
    public class ConstantSaveGameResult
    {
        private List<Model.GameResult> cacheGameResult = new List<Model.GameResult>();
        private DateTime lastSavedData;
     
        private const string RESULTS_KEY = "Resultados";
       
        public async Task SaveDatabaseAllGamesResults(List<Model.GameResult> GamesResults)
        {
            TimeSpan time = DateTime.Now - lastSavedData;
            if (time.Minutes > 2)
            {
                //salvar alterações

                //atualizar a ultima atualização
                lastSavedData = DateTime.Now;
               // memoryCache.Remove("Resultados");

            } 
        }

        public async Task SaveNewGameResult (Model.GameResult Result)
        { 
            cacheGameResult.Add(Result);
            await SaveDatabaseAllGamesResults(cacheGameResult);
        }

        public DateTime GetLastSavedData()
        {
            return  lastSavedData;
        }

        public List<Model.GameResult> GetCachedGameResults()
        {
            return null;
        }
    }
}
