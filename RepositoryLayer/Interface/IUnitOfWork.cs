namespace LDN.Framework.GenericRepository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Start a new transaction.
        /// </summary>
        Task<bool> BeginTransaction();

        /// <summary>
        /// Save changes to our database.
        /// </summary>
        Task<bool> Commit();
    }
}
