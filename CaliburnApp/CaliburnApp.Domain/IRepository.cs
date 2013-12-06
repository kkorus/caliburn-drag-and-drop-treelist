using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliburnApp.Domain.Entities;

namespace CaliburnApp.Domain
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity GetById(object id);
        IEnumerable<TEntity> Items();

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>Number of rows affected.</returns>
        int SaveChanges();
    }
}
