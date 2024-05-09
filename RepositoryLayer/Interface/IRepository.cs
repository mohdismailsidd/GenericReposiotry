using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface IRepository<TEntity> :  IDisposable where TEntity : class
    {
        /// <summary>
        /// Insert a new entity to our repository.
        /// <para>Examples:</para>
        /// <para>_repository.Insert(newEntity);</para>
        /// </summary>
        /// <param name="entity">Entity instance to be saved to our repository.</param>
        void Create(TEntity entity);

        /// <summary>
        /// Method to update a single entity.
        /// <para>Examples:</para>
        /// <para>_repository.Update(entity);</para>
        /// </summary>
        /// <param name="entity">Entity instance to be saved to our repository.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete an entity from our repository.
        /// <para>Examples:</para>
        /// <para>_repository.Delete(entity);</para>
        /// </summary>
        /// <param name="entity">Entity instance to be deleted to our repository.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Select an entity using it's primary keys as search criteria.
        /// <para>Examples:</para>
        /// <para>_repository.SelectByKey(userId);</para>
        /// <para>_repository.SelectByKey(companyId, userId);</para>
        /// </summary>
        /// <param name="primaryKeys">Primary key properties of our entity.</param>
        /// <returns>Returns an entity from our repository.</returns>
        TEntity GetByKey(params object[] primaryKeys);

        /// <summary>
        /// Select all entities from our repository
        /// <para>Examples:</para>
        /// <para>_repository.SelectAll();</para>
        /// </summary>
        /// <returns>Returns all entities from our repository.</returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// Select an entity from our repository using a filter.
        /// <para>Examples:</para>
        /// <para>_repository.Select(p=> p.UserId == 1);</para>
        /// <para>_repository.Select(p=> p.UserName.Contains("John") == true);</para>
        /// </summary>
        /// <param name="predicate">Filter applied to our search.</param>
        /// <returns>Returns an entity from our repository.</returns>
        TEntity Select(Expression<Func<TEntity, bool>> predicate);
    }
}
