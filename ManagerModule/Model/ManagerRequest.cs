using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerModule.Model
{
    public class ExcecutiveRequest
    {
        public string ExcecutiveName { get; set; }
        public int ContactNumber { get; set; }
        public string Locality { get; set; }
        public string EmailId { get; set; }
    }

    
    public class LocalityType
    {
        public string Locality { get; set; }
    }
    public class Customer
    {
        public int CustomerId{ get; set; }
    }
}
