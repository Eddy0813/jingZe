using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JingZeServer.Model
{
    public class selectjson
    {
        public string formid {  get; set; }
        public RequestData data { get; set; } = new RequestData();


    }

    public class RequestData
    {
        public int CreateOrgId { get; set;}
        public string Number { get; set;}
        public string IsSortBySeq { get; set; }
    }
}
