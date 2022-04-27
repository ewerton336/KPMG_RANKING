using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankingAPIWeb.Model
{
    public class GameResult
    {
        public int PlayerId { get; set; }
        public long GameId { get; set; }
        public int WinScore { get; set; }
        public DateTime GameDate { get; set; }
    }
}
