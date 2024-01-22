namespace StormyLib.DbInteraction;

public interface IDbInteraction<T> where T : class
{
    Task<T?> GetAsync(int id);
    Task<T?> AddAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<T?> DeleteAsync(int id);
}
