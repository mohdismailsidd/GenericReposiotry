using DataAccessLayer.Interface;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository
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

        public void Create(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IList<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public TEntity GetByKey(params object[] primaryKeys)
        {
            return DbSet.Find(primaryKeys);
        }

        public TEntity Select(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate)
                        .FirstOrDefault();
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}
