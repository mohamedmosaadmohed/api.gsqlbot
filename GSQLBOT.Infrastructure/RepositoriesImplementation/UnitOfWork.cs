

using GSQLBOT.Core.Repositories;
using GSQLBOT.Infrastructure.Data;

namespace GSQLBOT.Infrastructure.RepositoriesImplementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _;
        public IChatRepository Chat { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _ = context;
            Chat = new ChatRepository(context);
        }
        public Task CompleteAsync()
        {
            return _.SaveChangesAsync();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
