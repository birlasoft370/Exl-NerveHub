using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessLayer.CacheData;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using Microsoft.Graph;
using System.Collections;

namespace BPA.EmailManagement.BusinessLayer.ExchangeData
{
    public class MicrosoftGraph : IMailServiceGraph
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Get mailbox folder list
        /// </summary>
        /// <param name="oMailConfig">business entity BEMailConfiguration</param>
        /// <param name="oTenant">business entity BETenant</param>
        /// <returns>list of folder in mailbox</returns>
        public async Task<Hashtable> GetMailFolderListGraph(BEMailConfiguration oMailConfig, BETenant oTenant)
        {
            var hshTable = new Hashtable();
            GraphServiceClient service = null;
            try
            {
                using (MicrosoftGraphCache omail = new MicrosoftGraphCache())
                {
                    service = omail.GetGraphServiceClient(oMailConfig);
                }
                List<QueryOption> options = new List<QueryOption>();
                //options.Add(new QueryOption("$expand", "childFolders"));
                options.Add(new QueryOption("$top", "100"));
                //options.Add(new QueryOption("$includeHiddenFolders", "true"));

                // var user = await service.Me.Request().GetAsync();
                //var children = await service.Users[user.Id].MailFolders.Request(options).GetAsync();

                //var user = await service.Me.Request().GetAsync();
                var children = await service.Users[oMailConfig.sUserID.Trim()].MailFolders.Request(options).GetAsync();
                // var children = await service.Me.MailFolders.Request(options).GetAsync();

                foreach (var strval in children)
                {
                    if (strval.ChildFolderCount > 0)
                    {
                        hshTable.Add(strval.Id.ToString(), strval);
                    }
                    else
                    {
                        hshTable.Add(strval.Id.ToString(), strval);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                service = null;
            }

            return hshTable;
        }

        public async Task<Hashtable> GetFolderListGraph(BEMailConfiguration oMailConfig, string folderID, string rootFolderName, BETenant oTenant)
        {
            var hshTable = new Hashtable();
            GraphServiceClient service = null;
            try
            {
                using (MicrosoftGraphCache omail = new MicrosoftGraphCache())
                {
                    service = omail.GetGraphServiceClient(oMailConfig);
                }
                List<QueryOption> options = new List<QueryOption>();
                //options.Add(new QueryOption("$expand", "childFolders"));
                options.Add(new QueryOption("$top", int.MaxValue.ToString()));
                //options.Add(new QueryOption("$includeHiddenFolders", "true"));

                // var user = await service.Me.Request().GetAsync();
                //var children = await service.Users[user.Id].MailFolders.Request(options).GetAsync();

                //var user = await service.Me.Request().GetAsync();

                var children = await (oMailConfig.bEMSWebServerHosting == true ? service.Users[oMailConfig.sUserID.Trim()].MailFolders[folderID].ChildFolders.Request(options).GetAsync() : service.Me.MailFolders[folderID].ChildFolders.Request(options).GetAsync());


                //var childFolders = await service.Me.MailFolders["{mailFolder-id}"].ChildFolders
                //                                    .Request(queryOptions)
                //                                    .GetAsync();

                foreach (var strval in children)
                {
                    hshTable.Add(strval.Id.ToString(), strval);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                service = null;
            }

            return hshTable;
        }
    }
}
