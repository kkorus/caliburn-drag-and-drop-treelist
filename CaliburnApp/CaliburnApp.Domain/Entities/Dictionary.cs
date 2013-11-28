using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnApp.Domain.Entities
{
    [Table("Dictionaries")]
    public class Dictionary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<DictionaryItem> Items { get; set; }

        public Dictionary()
        {
            Items = new List<DictionaryItem>();
        }
    }
}
