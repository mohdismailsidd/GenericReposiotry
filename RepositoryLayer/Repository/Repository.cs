using LDN.Framework.GenericRepository.Interface;
using Microsoft.EntityFrameworkCore;

namespace LDN.Framework.GenericRepository.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDTOs"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TDTOs, TEntity> : IRepository<TDTOs> where TDTOs : class where TEntity : class
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
        protected virtual IEnumerable<TEntity> TransformDTOToEntity(IEnumerable<TDTOs> dtos)
        {
            List<TEntity> entity = new List<TEntity>();
            if (dtos != null)
            {
                // need to implement  in  specialise  class
            }
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TDTOs> TransformEntityToDTOs(IEnumerable<TEntity> entity)
        {
            List<TDTOs> dtos = new List<TDTOs>();
            if (entity != null)
            {
                // need to implement  in  specialise  class
            }
            return dtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        protected virtual bool ValidateDTOsState(TDTOs dtos)
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
        public virtual async Task<Task> CreateAsync(TDTOs dtos)
        {
            if (ValidateDTOsState(dtos))
            {
                _dbSet.Add(TransformDTOToEntity(new List<TDTOs> { dtos }).FirstOrDefault());
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
        public virtual async Task<Task> DeleteAsync(TDTOs dtos)
        {
            if (ValidateDTOsState(dtos))
            {
                _dbSet.Remove(TransformDTOToEntity(new List<TDTOs> { dtos }).FirstOrDefault());
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
        public virtual async Task<IEnumerable<TDTOs>> GetAllAsync()
        {
            return await Task.FromResult(TransformEntityToDTOs(_dbSet.ToList()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <returns></returns>
        public virtual async Task<TDTOs> GetByKeyAsync(params object[] primaryKeys)
        {
            return await Task.FromResult(TransformEntityToDTOs(new List<TEntity>() { _dbSet.Find(primaryKeys) }).FirstOrDefault());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        /// <exception cref="InvalidStateException"></exception>
        public virtual async Task<Task> UpdateAsync(TDTOs dtos)
        {
            if (ValidateDTOsState(dtos))
            {
                _dbSet.Update(TransformDTOToEntity(new List<TDTOs> { dtos }).FirstOrDefault());
                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidStateException("Dtos is invalid state");
            }
        }
    }
}
