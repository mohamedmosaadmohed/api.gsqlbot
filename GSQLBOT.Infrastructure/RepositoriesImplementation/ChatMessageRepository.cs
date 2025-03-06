using GSQLBOT.Core.Model;
using GSQLBOT.Core.Repositories;
using GSQLBOT.Infrastructure.Data;

namespace GSQLBOT.Infrastructure.RepositoriesImplementation
{
    public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(ApplicationDbContext context) : base(context) { }
    }
}
