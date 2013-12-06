using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnApp.Domain.Entities
{
    [Table("DictionaryItems")]
    public class DictionaryItem : Entity
    {
        public string Name { get; set; }
        public Dictionary Dictionary { get; set; }
    }
}
