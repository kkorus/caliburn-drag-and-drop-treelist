using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliburnApp.Domain.Entities;

namespace CaliburnApp.DataAccess.Mappings
{
    public class BusinessValueObjectMap : EntityTypeConfiguration<BusinessValueObject>
    {
        public BusinessValueObjectMap()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name);

            this.HasMany(x => x.Children)
                .WithOptional(y => y.Parent)
                .Map(m => m.MapKey("ParentId"));
        }
    }
}
