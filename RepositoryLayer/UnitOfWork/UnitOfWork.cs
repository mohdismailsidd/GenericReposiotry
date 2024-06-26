﻿using LDN.Framework.GenericRepository.Interface;
using LDN.Framework.GenericRepository.Resources;
using Microsoft.EntityFrameworkCore;
using System;

namespace LDN.Framework.GenericRepository.UnitOfWork
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
        public async Task<bool> BeginTransaction()
        {
            _disposed = false;
            return await Task.FromResult(true);
        }

        #endregion

        #region Commit

        /// <summary>
        /// Save changes to our database.
        /// </summary>
        public async Task<bool> Commit()
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    await DetachAllAsync();

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
        private async Task<bool> DetachAllAsync()
        {
            foreach (var dbEntityEntry in _dbContext.ChangeTracker.Entries().ToArray())
            {
                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
            return await Task.FromResult(true);
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
