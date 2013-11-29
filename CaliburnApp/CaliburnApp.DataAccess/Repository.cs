using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliburnApp.Domain;

namespace CaliburnApp.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Items()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}