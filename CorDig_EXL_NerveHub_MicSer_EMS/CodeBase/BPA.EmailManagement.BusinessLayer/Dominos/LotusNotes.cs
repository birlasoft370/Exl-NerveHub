using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using Domino;
using System.Collections;

namespace BPA.EmailManagement.BusinessLayer.Dominos
{
    //[ExceptionShielding("WCF Exception Shielding")]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class LotusNotes : IMailService
    {
        NotesSession ns;
        NotesDatabase ndb;
        NotesDocument nDoc;

        public Hashtable GetFolderList(BEMailConfiguration oMailConfig, string folderID, string rootFolderName, BETenant oTenant)
        {
            Hashtable hTbl = null;
            if (ndb == null)
            {
                ndb = LotusUtility.ConnectToLotusServer(oMailConfig.sLotusServerPath, oMailConfig.sNFSFilePath, oMailConfig.sPassword, oMailConfig.iCampaignID);
            }

            if (ndb != null)
            {
                Domino.NotesView nv = ndb.GetView("$ALL");
                object[] vws = ndb.Views as object[];
                hTbl = new Hashtable();
                if (vws != null)
                    for (int i = 0; i < vws.Length; i++)
                    {
                        if (((dynamic)vws[i]).IsFolder)
                        {
                            hTbl.Add(((dynamic)vws[i]).UniversalID, ((dynamic)vws[i]).Name);
                        }
                    }

            }
            return hTbl;
        }
        public LotusNotes(string UserPassword, string ServerName, string NSFFilePath, int compaignId, bool isCallFromConfiguration = false)
        {
            ndb = LotusUtility.ConnectToLotusServer(ServerName, NSFFilePath, UserPassword, compaignId, isCallFromConfiguration);
        }

        public LotusNotes()
        {

        }
        ~LotusNotes()
        {
            GC.Collect();
        }

        public void Dispose()
        {
            if (nDoc != null) nDoc = null;
            if (ndb != null) ndb = null;
            if (ns != null) ns = null;
            // GC.Collect();
        }

        public string test()
        {
            return "Lotus Test";
        }

        #region GetMailFolderList
        public Hashtable GetMailFolderList(BEMailConfiguration oMailConfig, BETenant oTenant)
        {
            Hashtable hTbl = null;
            if (ndb == null)
            {
                ndb = LotusUtility.ConnectToLotusServer(oMailConfig.sLotusServerPath, oMailConfig.sNFSFilePath, oMailConfig.sPassword, oMailConfig.iCampaignID);
            }

            if (ndb != null)
            {
                Domino.NotesView nv = ndb.GetView("$ALL");
                object[] vws = ndb.Views as object[];
                hTbl = new Hashtable();
                if (vws != null)
                    for (int i = 0; i < vws.Length; i++)
                    {
                        if (((dynamic)vws[i]).IsFolder)
                        {
                            hTbl.Add(((dynamic)vws[i]).UniversalID, ((dynamic)vws[i]).Name);
                        }
                    }

            }
            return hTbl;

        }

        #endregion
    }
}
