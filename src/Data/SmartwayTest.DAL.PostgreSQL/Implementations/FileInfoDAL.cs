using SmartwayTest.DAL.Connections;
using SmartwayTest.DAL.Helpers;
using SmartwayTest.DAL.Interfaces;
using SmartwayTest.DAL.Models;

namespace SmartwayTest.DAL.PostgreSQL.Implementations
{
    public class FileInfoDAL : IFileInfoDAL
    {
        private readonly IDbConnectionFabric _dbConnectionFabric;

        public FileInfoDAL(IDbConnectionFabric dbConnectionFabric)
        {
            _dbConnectionFabric = dbConnectionFabric;
        }

        public async Task AddAsync(FileInfoModel model)
        {
            string sql =
                @"
                INSERT INTO FileInfo (FileName, FilePath)
                VALUES (@FileName, @FilePath);
                ";
            await DbHelper.ExecuteAsync(_dbConnectionFabric, sql, model);
        }

        public async Task<FileInfoModel?> GetByIdAsync(int fileInfoId)
        {
            string sql =
                @"
                SELECT FileInfoId, FileName, FilePath
                FROM FileInfo
                WHERE FileInfoId = @fileInfoId; 
                ";
            return await DbHelper.QueryScalarAsync<FileInfoModel>(
                _dbConnectionFabric,
                sql,
                new { fileInfoId }
            );
        }

        public async Task<IEnumerable<FileInfoModel>> GetFilesAsync()
        {
            string sql =
                @"
                SELECT FileInfoId, FileName, FilePath
                FROM FileInfo;
                ";
            return await DbHelper.QueryAsync<FileInfoModel>(_dbConnectionFabric, sql);
        }
    }
}
