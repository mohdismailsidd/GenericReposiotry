using GenericRepository.Interface;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        public DbSet<TEntity> DbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
            try
            {
                DbSet = dbContext.Set<TEntity>();
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
        }

        public async  Task<Task> CreateAsync(TEntity entity)
        {
            DbSet.Add(entity);
            return Task.CompletedTask;
        }

        public async Task<Task> DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(DbSet.ToList());
        }

        public async Task<TEntity> GetByKeyAsync(params object[] primaryKeys)
        {
            return await Task.FromResult(DbSet.Find(primaryKeys));
        }

        public async Task<IList<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(DbSet.Where(predicate).ToList());
        }

        public async Task<Task> UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}
