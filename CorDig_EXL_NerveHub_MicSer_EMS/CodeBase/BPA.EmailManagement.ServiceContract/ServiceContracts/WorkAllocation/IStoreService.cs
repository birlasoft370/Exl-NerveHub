using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BPA.EmailManagement.ServiceContract.ServiceContracts.WorkAllocation
{
    [ServiceContract(Name = "StoreServiceContract")]
    public interface IStoreService : IDisposable
    {
        List<BEWorkObjectTAB> GetTabMasterList(int iStoreID, BETenant oTenant);

    }
}
