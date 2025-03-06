namespace GSQLBOT.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IChatRepository Chat { get; }
        IChatMessageRepository ChatMessage { get; }
        Task CompleteAsync();
    }
}
