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
        public async Task<IEnumerable<Model.GameResult>> GetTop100Players()
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveAllGamesScore(Model.GameResult gameResult)
        {
            try
            {
                //criado um foreach pois o método de inserção em massa facilita SQL injection
                    var sql = @"INSERT INTO isangue_ewertondev.kpmg_ranking 
                                (PLAYER_ID, 
                                GAME_SCORE, 
                                GAME_DATE) VALUES(
                                @PLAYERID, 
                                @GAMESCORE, 
                                @GAMEDATE);";

                    await DbConnection.ExecuteAsync(sql, new
                    {
                        PLAYERID = gameResult.PlayerId,
                        GAMESCORE = gameResult.GameScore,
                        GAMEDATE = gameResult.GameDate
                    }
                    );
                
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
