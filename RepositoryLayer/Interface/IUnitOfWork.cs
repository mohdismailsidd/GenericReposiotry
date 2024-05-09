using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.Interface
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
