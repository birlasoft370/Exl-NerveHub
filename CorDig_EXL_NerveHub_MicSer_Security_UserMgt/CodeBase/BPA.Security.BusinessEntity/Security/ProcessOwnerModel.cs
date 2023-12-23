using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessEntity.Security
{
    public class ProcessOwnerModel
    {
        public string ClientName { get; set; }
        public string Approver { get; set; }
        public string ProcessName { get; set; }
      public string UserId { get; set; }
        public List<string> ProcessOwnerName { get { return _ProcessOwnerName; } set { _ProcessOwnerName = value; } }

        List<string> _ProcessOwnerName = new List<string>();
    }
}
