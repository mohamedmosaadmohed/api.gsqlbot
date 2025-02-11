using System.Linq.Expressions;

namespace GSQLBOT.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // _context.TbChat.Where(c => c.Id == Id).ToList();
        // _context.TbChat.Include("Text").ToList();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, string? IncludeWord = null);

        // _context.TbChat.Where(c => c.Id == Id).ToList();
        // _context.TbChat.Include("Text").ToList();
        Task<T> GetFirstorDefaultAsync(Expression<Func<T, bool>>? expression = null, string? IncludeWord = null);
        // _context.TbCatagory.Add(catagory);
        Task AddAsync(T entity);
        // _context.TbChat.Remove(item);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
