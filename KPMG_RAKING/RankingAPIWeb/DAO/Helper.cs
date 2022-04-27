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
        public static System.Data.IDbConnection DBConnectionOracle
        {
            get
            {
                return new Oracle.ManagedDataAccess.Client.OracleConnection(_ambiente);
            }
        }
    }
}
