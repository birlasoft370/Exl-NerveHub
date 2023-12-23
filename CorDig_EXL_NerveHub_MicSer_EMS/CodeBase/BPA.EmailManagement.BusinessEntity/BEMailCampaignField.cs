using BPA.EmailManagement.BusinessEntity.ExternalRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessEntity
{
    /// <summary>
    /// Mail Campaign Addtion fields
    /// </summary>
    public class BEMailCampaignField : ObjectBase
    {
        public string ObjName { get; set; }
        public string DataType { get; set; }
    }
}
