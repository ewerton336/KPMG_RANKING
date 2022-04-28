using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankingAPIWeb.DAO
{
    public class Helper
    {
        public static string _ambiente { get; set; }

        public Helper(string ambiente)
        {
            ambiente = _ambiente;
        }
        public static MySqlConnection DBConnectionMySql
        {
            get
            {
                return new MySqlConnection(_ambiente);
            }
        }
    }
}
