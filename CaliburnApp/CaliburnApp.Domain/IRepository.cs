using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnApp.Domain
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Items();
    }
}
