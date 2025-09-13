using Dapper;

namespace F4ConversationCloud.Infrastructure.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<int> GetCountAsync(string spName, DynamicParameters parameters);
    Task<IEnumerable<T>> GetAllAsync(string spName);
    Task<T?> GetByIdAsync(string spName, DynamicParameters parameters);
    Task<IEnumerable<T>> GetListByValuesAsync(string spName, DynamicParameters parameters);
    Task<IEnumerable<U>> GetListByValuesAsync<U>(string spName, DynamicParameters parameters);
    Task<T?> GetByValuesAsync(string spName, DynamicParameters parameters);
    Task<U?> GetByValuesAsync<U>(string spName, DynamicParameters parameters);
    Task<int> InsertUpdateAsync(string spName, DynamicParameters parameters);
    Task<bool> InsertMultipleAsync(string spName, DynamicParameters parameters);
    Task<int> UpdateValuesAsync(string spName, DynamicParameters parameters);
    Task<bool> DeleteAsync(string spName, DynamicParameters parameters);
    Task<bool> RestoreAsync(string spName, DynamicParameters parameters);
    Task<IEnumerable<T>> GetListByParamAsync<T>(string spName, DynamicParameters parameters);
    Task<bool> CheckExistAsync(string spName, DynamicParameters parameters);
}

