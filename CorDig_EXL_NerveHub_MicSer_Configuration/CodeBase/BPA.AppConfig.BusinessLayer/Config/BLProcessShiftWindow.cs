using BPA.AppConfig.BusinessEntity.Config;
using BPA.AppConfig.BusinessEntity;
using BPA.AppConfig.ServiceContracts.ServiceContracts.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA.AppConfig.BusinessEntity.Application;
using BPA.AppConfig.Datalayer.Config;

namespace BPA.AppConfig.BusinessLayer.Config
{
    public class BLProcessShiftWindow : IProcessShiftWindowService, IDisposable, IDataOperation<BEProcessShiftWindow>
    {
        public BLProcessShiftWindow()
        { }

        public void Dispose()
        { }

        public List<BEProcessConfigData> GetProcessShiftConfig(string sProcessName, BETenant oTenant)
        {
            List<BEProcessConfigData> lstConfig = new List<BEProcessConfigData>();
            using (DLProcessShiftWindow objProcessShiftWindow = new DLProcessShiftWindow(oTenant))
            {
                lstConfig = DataSetToList.ConvertTo<BEProcessConfigData>(objProcessShiftWindow.GetProcessShiftConfig(sProcessName));
            }
            return lstConfig;
        }

        public BEProcessWindow GetProcessShiftConfigDetail(int iProcessshiftConfigId, BETenant oTenant)
        {
            BEProcessWindow objProcessWindow = new BEProcessWindow();

            //List<BEProcessConfigData> lstConfig = new List<BEProcessConfigData>();
            using (DLProcessShiftWindow objProcessShiftWindow = new DLProcessShiftWindow(oTenant))
            {
                objProcessWindow.lstProcessConfigData = DataSetToList.ConvertTo<BEProcessConfigData>(objProcessShiftWindow.GetProcessShiftConfigDetail(iProcessshiftConfigId).Tables[0]);
                objProcessWindow.lstProcessBreakData = DataSetToList.ConvertTo<BEProcessBreakData>(objProcessShiftWindow.GetProcessShiftConfigDetail(iProcessshiftConfigId).Tables[1]);
            }
            return objProcessWindow;
        }       

        public int InsertProcessSiftConfig(string strBreakXml, BETenant oTenant)
        {
            using (DLProcessShiftWindow objProcessShiftWindow = new DLProcessShiftWindow(oTenant))
            {
                return objProcessShiftWindow.InsertProcessSiftConfig(strBreakXml);
            }
        }        

        public int UpdateProcessSiftConfig(string strBreakXml, BETenant oTenant)
        {
            using (DLProcessShiftWindow objProcessShiftWindow = new DLProcessShiftWindow(oTenant))
            {
                return objProcessShiftWindow.UpdateProcessSiftConfig(strBreakXml);
            }
        }

        public void InsertData(BEProcessShiftWindow ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }

        public void UpdateData(BEProcessShiftWindow ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }
        public void DeleteData(BEProcessShiftWindow ClObject, int iFormID, BETenant oTenant)
        {
            throw new NotImplementedException();
        }
    }
}
