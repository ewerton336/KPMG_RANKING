using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;


namespace RankingAPIWeb.DAO
{
    public class DaoGameResult : DaoConexao
    {
        public DaoGameResult(MySqlConnection dbConnection) : base(dbConnection)
        {
        }
        public async Task<IEnumerable<Model.GameResult>> GetTop100Players ()
        {
            try
            {
                var sql = @"SELECT GAME_ID GameId,
                            PLAYER_ID PlayerId,
                            GAME_SCORE GameScore,
                            GAME_DATE GameDate
                            from kpmg_ranking";
                return await DbConnection.QueryAsync<Model.GameResult>(sql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
