using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnApp.Domain.Entities
{
    [Table("BusinessValueObjects")]
    public class BusinessValueObject : Entity
    {
        public string Name { get; set; }
        public BusinessValueObject Parent { get; set; }
        public ICollection<BusinessValueObject> Children { get; set; }

        public BusinessValueObject()
        {
            Children = new List<BusinessValueObject>();
        }

        public void AddChild(BusinessValueObject child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public void ChangeParent(BusinessValueObject parent)
        {
            Parent = parent;
            Parent.Children.Add(this);
        }
    }
}
