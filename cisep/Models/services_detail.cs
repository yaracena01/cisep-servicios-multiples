using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.Models
{
    public class services_detail
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Id_Services { get; set; }
        public decimal Price { get; set; }
        public Services Services { get; set; }
    }
}
