namespace GSQLBOT.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IChatRepository Chat { get; }
        Task CompleteAsync();
    }
}
