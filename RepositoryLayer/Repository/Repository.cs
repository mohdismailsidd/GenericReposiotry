using LDN.Framework.GenericRepository.Exceptions;
using LDN.Framework.GenericRepository.Interface;
using Microsoft.EntityFrameworkCore;

namespace LDN.Framework.GenericRepository.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDTOs"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        public DbSet<TEntity> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
            try
            {
                _dbSet = dbContext.Set<TEntity>();
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        protected virtual bool ValidateDTOsState(TEntity entity)
        {
            return true;
            // dtos state validation
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        /// <exception cref="InvalidStateException"></exception>
        public virtual async Task<Task> CreateAsync(TEntity entity)
        {
            if (ValidateDTOsState(entity))
            {
                _dbSet.Add(entity);
                _dbContext.SaveChanges();
                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidStateException("Dtos is invalid state");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        /// <exception cref="InvalidStateException"></exception>
        public virtual async Task<Task> DeleteAsync(TEntity entity)
        {
            if (ValidateDTOsState(entity))
            {
                _dbSet.Remove(entity);
                _dbContext.SaveChanges();
                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidStateException("Dtos is invalid state");
            }
        }

        /// <summary>
        ///      Performs application-defined tasks associated with freeing, releasing, or resetting
        ///      unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(_dbSet.ToList().AsQueryable());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByKeyAsync(params object[] primaryKeys)
        {
            return await Task.FromResult(_dbSet.Find(primaryKeys));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        /// <exception cref="InvalidStateException"></exception>
        public virtual async Task<Task> UpdateAsync(TEntity entity)
        {
            if (ValidateDTOsState(entity))
            {
                _dbSet.Update(entity);
                _dbContext.SaveChanges();
                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidStateException("Dtos is invalid state");
            }
        }
    }
}
