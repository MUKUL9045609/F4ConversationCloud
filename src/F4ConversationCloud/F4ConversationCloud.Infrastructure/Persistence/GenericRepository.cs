using Dapper;
using F4ConversationCloud.Infrastructure.Interfaces;
using System.Data;

namespace F4ConversationCloud.Infrastructure.Persistence
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }
        public async Task<int> GetCountAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<int>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync(string spName)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<T>(spName, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<T?> GetByIdAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<T>> GetListByValuesAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<U>> GetListByValuesAsync<U>(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<U>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<T?> GetByValuesAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<U?> GetByValuesAsync<U>(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<U>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> InsertUpdateAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    parameters.Add("userId", _context.SessionUserId);
                    return await connection.QueryFirstOrDefaultAsync<int>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> InsertMultipleAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<bool>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> UpdateValuesAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<int>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<bool>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> RestoreAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<bool>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<U>> GetListByParamAsync<U>(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<U>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> CheckExistAsync(string spName, DynamicParameters parameters)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<bool>(spName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }

}
