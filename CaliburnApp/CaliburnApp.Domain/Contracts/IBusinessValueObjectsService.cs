using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliburnApp.Domain.Entities;

namespace CaliburnApp.Domain.Contracts
{
    public interface IBusinessValueObjectsService
    {
        /// <summary>
        /// Itemses this instance.
        /// </summary>
        /// <returns></returns>
        IEnumerable<BusinessValueObject> Items();

        /// <summary>
        /// Changes the parent.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="delete">if set to <c>true</c> [delete].</param>
        void ChangeParent(int id, int parentId, bool isCopying = false);
    }
}
