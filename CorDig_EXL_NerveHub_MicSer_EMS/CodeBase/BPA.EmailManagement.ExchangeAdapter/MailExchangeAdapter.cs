using System.Net.Mail;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace BPA.EmailManagement.ExchangeAdapter
{
    public static class MailExchangeAdapter
    {
        /// <summary>
        /// Connect to exchange service.
        /// </summary>
        /// <param name="emailid"> Mail ID</param>
        /// <param name="password">User Password</param>
        /// <param name="userid">User ID</param>
        /// <param name="DefaultCredentials">check if user will connect with the Service ID credential </param>
        /// <returns></returns>
        public static ExchangeService ConnectExchangeServer(string emailid, string password, string userid, bool DefaultCredentials, string sAutoDiscoveryPath)
        {
            ServicePointManager.ServerCertificateValidationCallback = CertificateCallback.CertificateValidationCallBack;
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
            try
            {
                service.WebProxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                service.WebProxy.Credentials = CredentialCache.DefaultCredentials;

                if (DefaultCredentials)
                {
                    //service will pick the application pool credentical for communication with shared mail box
                    service.UseDefaultCredentials = true;
                }
                else
                {
                    //connect with users credential
                    service.Credentials = new WebCredentials(userid, password);
                }
                service.Url = new Uri(sAutoDiscoveryPath.Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return service;

        }

        public static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// get mail message for the Item
        /// </summary>
        /// <param name="service">ExchangeService object</param>
        /// <param name="ID">Microsoft.Exchange.WebServices.Data.ItemId object</param>
        /// <param name="propertySet">Microsoft.Exchange.WebServices.Data object</param>
        /// <returns></returns>
        public static EmailMessage GetEmailMessage(ExchangeService service, ItemId ID, PropertySet propertySet)
        {
            EmailMessage message = EmailMessage.Bind(service, ID, propertySet);
            return message;
        }

        /// <summary>
        /// Find items in the share mailbox based on the searchfilter criteria 
        /// </summary>
        /// <param name="service">ExchangeService object </param>
        /// <param name="folderID">Microsoft.Exchange.WebServices.Data.FolderId FolderID</param>
        /// <param name="searchFilter">Microsoft.Exchange.WebServices.Data.SearchFilter Search criteria </param>
        /// <param name="itemView">Microsoft.Exchange.WebServices.Data.ItemView itemview</param>
        /// <returns></returns>
        public static FindItemsResults<Item> FindItems(ExchangeService service, FolderId folderID, SearchFilter searchFilter, ItemView itemView)
        {
            //service.ClientRequestId = Guid.NewGuid().ToString();
            //Folder frd = Folder.Bind(service, folderID);
            //frd.Service.ClientRequestId = Guid.NewGuid().ToString();
            //FindItemsResults<Item> findResults = frd.FindItems(searchFilter, itemView);
            FindItemsResults<Item> findResults = service.FindItems(folderID, searchFilter, itemView);

            return findResults;
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="EmailMessage">Microsoft.Exchange.WebServices.Data.EmailMessage EmailMessage</param>
        public static void SendMail(EmailMessage emailMessage)
        {
            emailMessage.Send();
        }
        public static void SendMailAndSaveCopy(EmailMessage emailMessage)
        {
            emailMessage.SendAndSaveCopy();
        }

        public static FindFoldersResults GetFoldersList(ExchangeService service, FolderId folderid)
        {
            Folder folders = Folder.Bind(service, folderid);
            folders.Load();

            return folders.FindFolders(new FolderView(100));
        }
        public static void SendMailAndSaveCopy(EmailMessage emailMessage, FolderId folderID)
        {
            emailMessage.SendAndSaveCopy(folderID);
        }
        public static void SendMailAndSaveCopy(EmailMessage emailMessage, WellKnownFolderName FolderName)
        {
            emailMessage.SendAndSaveCopy(FolderName);
        }

        public static void ForwardMail(EmailMessage emailMessage, MessageBody messageBody, EmailAddress[] toRecepents)
        {
            emailMessage.Forward(messageBody, toRecepents);
        }
        public static void Reply(EmailMessage emailMessage, MessageBody messageBody, bool replyToAll)
        {
            emailMessage.Reply(messageBody, replyToAll);
        }

        public static FolderId GetFolderID(ExchangeService service, WellKnownFolderName FolderName, string FolderDisplayName)
        {
            FolderId folderid = null;
            Folder rootfolder = Folder.Bind(service, FolderName);
            rootfolder.Load();

            foreach (Folder folder in rootfolder.FindFolders(new FolderView(100)))
            {
                if (folder.DisplayName.ToUpper().Trim() == FolderDisplayName.ToUpper().Trim())
                {
                    folderid = folder.Id;
                }
            }

            return folderid;
        }


        /// <summary>
        /// This function provides root and child level public folder 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        public static List<CompositeFolder> GetChildFoldersList(ExchangeService service, WellKnownFolderName FolderName)
        {
            CompositeFolder compositeFolder = null;
            List<CompositeFolder> lstcompositeFolder = new List<CompositeFolder>();
            Folder folders = Folder.Bind(service, FolderName);
            folders.Load();
            FindFoldersResults findFolderResults = folders.FindFolders(new FolderView(int.MaxValue));
            foreach (Folder TopLevelfolder in findFolderResults)
            {
                //Create the new Instance for every object 
                compositeFolder = new CompositeFolder();
                compositeFolder.Children = new List<Folder>();

                //Add the root item at top level
                compositeFolder.Children.Add(TopLevelfolder);

                //if child count is greater then zero then find the subfolder
                if (TopLevelfolder.ChildFolderCount > 0)
                    FindAllSubFolders(service, TopLevelfolder.Id, compositeFolder);
                //Add that folder to main list
                lstcompositeFolder.Add(compositeFolder);
            }
            return lstcompositeFolder;
        }

        /// <summary>
        /// This function is used to get the subfolder values.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="parentFolderId"></param>
        /// <param name="compositeFolder"></param>
        private static void FindAllSubFolders(ExchangeService service, FolderId parentFolderId, CompositeFolder compositeFolder)
        {
            //search for sub folders
            FolderView folderView = new FolderView(int.MaxValue);
            try
            {
                //Get the folder under root folder or child folder
                FindFoldersResults foundFolders = service.FindFolders(parentFolderId, folderView);

                // Add the list to the growing complete list
                compositeFolder.Children.AddRange(foundFolders);

                // Now recurse the function
                foreach (Folder folder in foundFolders)
                {
                    //Call the function again & again
                    FindAllSubFolders(service, folder.Id, compositeFolder);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*New method to read shared mailbox all items by Tarun*/
        public static FindFoldersResults GetAllFoldersList(ExchangeService service, FolderId folderid)
        {
            FindFoldersResults findFolderResults = service.FindFolders(folderid, new FolderView(int.MaxValue) { Traversal = FolderTraversal.Deep });
            return findFolderResults;
        }

        /// <summary>
        /// get the Folder list for non public folder.
        /// </summary>
        /// <param name="service">Exchange service Object</param>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        public static FindFoldersResults GetAllFoldersList(ExchangeService service, WellKnownFolderName FolderName)
        {

            FindFoldersResults findFolderResults = service.FindFolders(FolderName, new FolderView(int.MaxValue) { Traversal = FolderTraversal.Deep });
            return findFolderResults;
        }

        /// <summary>
        /// Get Folder List for Public Folder
        /// </summary>
        /// <param name="service">Exchange service Object</param>
        /// <param name="FolderName"></param>
        /// <returns>FindFoldersResults </returns>
        public static FindFoldersResults GetFoldersList(ExchangeService service, WellKnownFolderName FolderName)
        {
            Folder folders = Folder.Bind(service, FolderName);
            folders.Load();

            return folders.FindFolders(new FolderView(int.MaxValue));
        }

        public static Item MoveToFolder(EmailMessage emailMessage, FolderId fldId)
        {
            return emailMessage.Move(fldId);
        }

        public static void DeleteMail(EmailMessage emailMessage, DeleteMode dm)
        {
            emailMessage.Delete(dm);
        }
        public static NameResolutionCollection ListGlobalContacts(ExchangeService service, string UserName)
        {
            /* passing true as the third parameter to "ResolveName" is important to
               make sure you get the contact details as well as the mailbox details */
            return service.ResolveName(UserName, ResolveNameSearchLocation.DirectoryOnly, true);

        }
    }
    [Serializable]
    public class CompositeFolder
    {
        public string Name { get; set; }
        public List<Folder> Children { get; set; }
    }
}