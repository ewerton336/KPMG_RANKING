using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RankingAPIWeb.DAO
{
    public class DaoConexao : IDisposable
    {
        public IDbConnection DbConnection { get; private set; }
        public DaoConexao(IDbConnection dbConnection)
        {
            dbConnection.ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));User Id=system;Password=123456;";

            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            DbConnection = dbConnection;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
}
