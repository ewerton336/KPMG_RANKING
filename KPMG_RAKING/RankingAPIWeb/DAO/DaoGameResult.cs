using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace RankingAPIWeb.DAO
{
    public class DaoGameResult : DaoConexao
    {
        public DaoGameResult(IDbConnection dbConnection) : base(dbConnection)
        {
        }
        public async Task<IEnumerable<Model.GameResult>> GetTop100Players ()
        {
            var sql = @"SELECT ";
            return await DbConnection.QueryAsync<Model.GameResult>(sql);
        }

    }
}
