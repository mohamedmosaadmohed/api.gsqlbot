using GSQLBOT.Core.Model;
using GSQLBOT.Core.Repositories;
using GSQLBOT.Infrastructure.Data;

namespace GSQLBOT.Infrastructure.RepositoriesImplementation
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(ApplicationDbContext context) : base(context) { }
    }
}
