using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Start a new transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Save changes to our database.
        /// </summary>
        bool Commit();
    }
}
