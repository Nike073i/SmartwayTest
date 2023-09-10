using SmartwayTest.DAL.Connections;
using SmartwayTest.DAL.Helpers;
using SmartwayTest.DAL.Interfaces;
using SmartwayTest.DAL.Models;

namespace SmartwayTest.DAL.PostgreSQL.Implementations
{
    public class DownloadTokenDAL : IDownloadTokenDAL
    {
        private readonly IDbConnectionFabric _dbConnectionFabric;

        public DownloadTokenDAL(IDbConnectionFabric dbConnectionFabric)
        {
            _dbConnectionFabric = dbConnectionFabric;
        }

        public async Task<Guid> CreateAsync(int fileInfoId)
        {
            var tokenId = Guid.NewGuid();
            bool isUsed = false;
            string sql =
                @"
                INSERT INTO DownloadTokens (DownloadTokenId, FileInfoId, IsUsed)
                VALUES (@tokenId, @fileInfoId, @isUsed);
                ";
            await DbHelper.ExecuteAsync(
                _dbConnectionFabric,
                sql,
                new
                {
                    tokenId,
                    fileInfoId,
                    isUsed
                }
            );
            return tokenId;
        }

        public async Task<DownloadTokenModel?> GetLockAsync(Guid downloadTokenId)
        {
            string sql =
                @"
                SELECT DownloadTokenId, FileInfoId, IsUsed
                FROM DownloadTokens
                WHERE DownloadTokenId=@downloadTokenId
                FOR UPDATE;
                ";
            return await DbHelper.QueryScalarAsync<DownloadTokenModel?>(
                _dbConnectionFabric,
                sql,
                new { downloadTokenId }
            );
        }

        public async Task MakeAsUsedAsync(Guid downloadTokenId)
        {
            string sql =
                @"
                UPDATE DownloadTokens
                SET IsUsed = true
                WHERE DownloadTokenId = @downloadTokenId
                ";
            await DbHelper.ExecuteAsync(_dbConnectionFabric, sql, new { downloadTokenId });
        }
    }
}
