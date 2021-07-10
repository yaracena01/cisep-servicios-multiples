using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.Models
{
    public class Clients
    {
		public int Id { get; set; }
		public string First_name { get; set; }
		public string Last_name { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Phone { get; set; }
		public bool Notification { get; set; }

}
}
