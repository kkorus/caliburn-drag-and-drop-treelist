using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliburnApp.Domain;
using CaliburnApp.Domain.Entities;

namespace CaliburnApp.DataAccess
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> Items()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}