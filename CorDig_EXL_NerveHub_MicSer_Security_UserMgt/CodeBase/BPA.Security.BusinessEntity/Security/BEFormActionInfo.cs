using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessEntity.Security
{
    public class BEFormActionInfo
    {
        public string ModuleName { get; set; }
        public int FormID { get; set; }
        public string FormName { get; set; }
        public string Description { get; set; }
        public string View { get; set; }
        public string Modify { get; set; }
        public string Delete { get; set; }
        public string Approve { get; set; }
        public string Add { get; set; }
        public string ChildID { get; set; }

    }
}
