using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Resources;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext _dbContext;
        private bool _disposed;

        #region Properties

        /// <summary>
        /// Returns if our transaction was succesful.
        /// </summary>
        public bool OperationSuccesful { get; private set; }

        /// <summary>
        /// Message returned from our transaction.
        /// </summary>
        public string OperationMessage { get; private set; }

        #endregion

        #region Constructor

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
        }

        #endregion

        #region Begin Transaction

        /// <summary>
        /// Start a new transaction.
        /// </summary>
        public void BeginTransaction()
        {
            _disposed = false;
        }

        #endregion

        #region Commit

        /// <summary>
        /// Save changes to our database.
        /// </summary>
        public bool Commit()
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    DetachAll();

                    OperationSuccesful = true;
                    OperationMessage = Info.OperationSuccess;
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    OperationMessage = string.Format(Errors.Rollback_0, ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    OperationMessage = string.Format(Errors.Error_0, ex.Message);
                }
            }

            return OperationSuccesful;
        }

        #endregion

        #region Detach

        /// <summary>
        /// Detaches all entities from the context that were added or modified.
        /// </summary>
        private void DetachAll()
        {
            foreach (var dbEntityEntry in _dbContext.ChangeTracker.Entries().ToArray())
            {
                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose method.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
