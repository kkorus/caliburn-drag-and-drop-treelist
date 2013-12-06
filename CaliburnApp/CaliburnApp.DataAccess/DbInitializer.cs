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
    //public class DbInitializer : DropCreateDatabaseAlways<DatabaseContext>
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
            
            IDbSet<BusinessValueObject> objects = context.Set<BusinessValueObject>();
            var solution = new BusinessValueObject { Name = "Solution" };

            var businessObject1 = new BusinessValueObject { Name = "BO 1" };
            businessObject1.Children.Add(new BusinessValueObject { Name = "Child 1" });
            businessObject1.Children.Add(new BusinessValueObject { Name = "Child 2" });
            businessObject1.Children.Add(new BusinessValueObject { Name = "Child 3" });
            businessObject1.Children.Add(new BusinessValueObject { Name = "Child 4" });

            var businessObject2 = new BusinessValueObject { Name = "BO 2" };
            businessObject2.Children.Add(new BusinessValueObject { Name = "Child 1" });
            businessObject2.Children.Add(new BusinessValueObject { Name = "Child 2" });
            businessObject2.Children.Add(new BusinessValueObject { Name = "Child 3" });
            businessObject2.Children.Add(new BusinessValueObject { Name = "Child 4" });

            var businessObject3 = new BusinessValueObject { Name = "BO 3" };
            var children = new BusinessValueObject { Name = "Child 1" };
            children.Children.Add(new BusinessValueObject { Name = "Children 1" });
            businessObject3.Children.Add(children);

            solution.Children.Add(businessObject1);
            solution.Children.Add(businessObject2);
            solution.Children.Add(businessObject3);

            objects.Add(solution);
        }
    }
}
