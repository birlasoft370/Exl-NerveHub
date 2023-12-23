using BPA.Security.BusinessEntity.ExtrernalRefre;
using BPA.Security.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.Security.ServiceContract
{
    [ServiceContract(Name = "MenuServiceContract")]
    public interface IMenuService
    {
        //[OperationContract(Name = "GetMenu")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetMenu(int UserId, BETenant oTenant);

        //[OperationContract(Name = "GetMenuMoss")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetMenu(int UserId, bool isMossApplicationMenu, BETenant oTenant);

        //[OperationContract(Name = "GetMenuData")]
        //[FaultContract(typeof(ServiceFault))]
        //DataSet GetMenuData(int UserID, bool isMossApplicationMenu);

        //[OperationContract(Name = "GetSmartMenu")]
        //[FaultContract(typeof(ServiceFault))]
        DataSet GetSmartMenu(int UserID, bool isMossApplicationMenu, BETenant oTenant);

        //[OperationContract(Name = "GetSmartMenuData")]
        //[FaultContract(typeof(ServiceFault))]
        //DataSet GetSmartMenuData(int UserID, bool isMossApplicationMenu);

        //[OperationContract(Name = "GetRoleWiseMenu")]
        //[FaultContract(typeof(ServiceFault))]
        List<BEMenuItems> GetRoleWiseMenu(int UserID, bool isMossApplicationMenu, BETenant oTenant);
        DataSet GetLandingData(BETenant oTenant);
    }
}
