using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnApp.Domain.Entities
{
    public class DictionaryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary Dictionary { get; set; }
    }
}
