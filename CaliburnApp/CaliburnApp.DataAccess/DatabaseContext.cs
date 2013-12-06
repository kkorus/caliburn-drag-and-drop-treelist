using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using CaliburnApp.Domain.Entities;
using CaliburnApp.DataAccess.Mappings;

namespace CaliburnApp.DataAccess
{
    public class DatabaseContext : DbContext
    {
        private const string CONNECTION_STRING_NAME = "CaliburnApp";

        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<DictionaryItem> DictionaryItems { get; set; }
        public DbSet<BusinessValueObject> BusinessValueObjects { get; set; }

        public DatabaseContext() : base(CONNECTION_STRING_NAME)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BusinessValueObjectMap());
        }
    }
}
