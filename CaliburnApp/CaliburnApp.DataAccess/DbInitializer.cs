using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliburnApp.Domain.Entities;

namespace CaliburnApp.DataAccess
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Initilize(context);
            context.SaveChanges();
        }

        public void Initilize(DatabaseContext context)
        {
            IDbSet<Dictionary> dictionaries = context.Set<Dictionary>();

            var dictionary1 = new Dictionary { Name = "Dictionary 1" };
            dictionary1.Items.Add(new DictionaryItem { Name = "Item 1 1"});
            dictionary1.Items.Add(new DictionaryItem { Name = "Item 1 2"});
            dictionary1.Items.Add(new DictionaryItem { Name = "Item 1 3"});
            dictionary1.Items.Add(new DictionaryItem { Name = "Item 1 4"});
            dictionary1.Items.Add(new DictionaryItem { Name = "Item 1 5"});
            dictionary1.Items.Add(new DictionaryItem { Name = "Item 1 6"});
            dictionary1.Items.Add(new DictionaryItem { Name = "Item 1 7"});

            var dictionary2 = new Dictionary { Name = "Dictionary 2" };
            dictionary2.Items.Add(new DictionaryItem { Name = "Item 2 1" });
            dictionary2.Items.Add(new DictionaryItem { Name = "Item 2 2" });
            dictionary2.Items.Add(new DictionaryItem { Name = "Item 2 3" });
            dictionary2.Items.Add(new DictionaryItem { Name = "Item 2 4" });
            dictionary2.Items.Add(new DictionaryItem { Name = "Item 2 5" });
            dictionary2.Items.Add(new DictionaryItem { Name = "Item 2 6" });
            dictionary2.Items.Add(new DictionaryItem { Name = "Item 2 7" });

            dictionaries.Add(dictionary1);
            dictionaries.Add(dictionary2);
        }
    }
}
