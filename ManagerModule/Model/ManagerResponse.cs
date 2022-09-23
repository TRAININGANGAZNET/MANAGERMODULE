using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerModule.Model
{
    public class ManagerResponse
    {
        public string Responses { get; set; }
        public string id { get; set; }
    }
    public class GetExecresponse
    {
        public string ExcecutiveName { get; set; }
        public int ContactNumber { get; set; }
        public string Locality { get; set; }
        public string EmailId { get; set; }
    }

    public class AssignExecResponse
    {
        public int ExcutiveID { get; set; }
        public string Responses { get; set; }
    }
}
