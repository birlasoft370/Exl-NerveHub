using BPA.EmailManagement.BusinessEntity.ExternalRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessEntity
{
    [Serializable]
    [DataContract]
    public class BEMailHelper : ObjectBase, ICloneable
    {
        private Random rnd = new Random();
        private int _iID = 0;
        public BEMailHelper()
        {
            _iID = rnd.Next(1, 500);
        }

        [DataMember]
        public int iID { get { return _iID; } set { _iID = value; } }

        [DataMember]
        public int iWorkID { get; set; }

        [DataMember]
        public string sMailUniqueID { get; set; }

        [DataMember]
        public string currentUserEmailId { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string UserPassword { get; set; }


        [DataMember]
        public string FromAddress { get; set; }
        [DataMember]
        public string FromAddressWithDisplayName { get; set; }

        [DataMember]
        public string MsgSubject { get; set; }
        [DataMember]
        public List<string> MailToRecipients { get; set; }
        [DataMember]
        public List<string> MailCCRecipients { get; set; }
        [DataMember]
        public List<string> MailBCCRecipients { get; set; }

        [DataMember]
        public string sImportant { get; set; }
        [DataMember]
        public string sSensitivity { get; set; }


        /// <summary>
        /// Orignal mail body content
        /// </summary>
        [DataMember]
        public string MsgBody { get; set; }

        /// <summary>
        /// New body content entered by User
        /// </summary>
        [DataMember]
        public string DraftMsgBody { get; set; }
        [DataMember]
        public List<MailAttachment> OMailAttachment { get; set; }

        [DataMember]
        public string ConversationID { get; set; }
        [DataMember]
        public string ExtProperties { get; set; }

        [DataMember]
        public string movefolderName { get; set; }

        [DataMember]
        public string movefolderID { get; set; }

        [DataMember]
        public string mailFolderName { get; set; }

        [DataMember]
        public string MsgId { get; set; }
        [DataMember]
        public DateTime Msg_DateReceived { get; set; }


        [DataMember]
        public string MsgFileName { get; set; }
        [DataMember]
        public string MsgSender { get; set; }
        [DataMember]
        public string MsgSenderDisplayName { get; set; }
        [DataMember]
        public List<string> mMailName { get; set; }

        [DataMember]
        public bool bChecked { get; set; }

        [DataMember]
        public bool bOutLook { get; set; }

        [DataMember]
        public ReplyType replyType { get; set; }

        [DataMember]
        public bool IsBackOffice { get; set; }

        [DataMember]
        public Byte[] MailData { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    [Serializable]
    [DataContract]
    public class MailAttachment
    {
        private Guid _iD;
        int _Randomnumber = 0;
        public MailAttachment()
        {

            Random rand = new Random();
            _Randomnumber = rand.Next(50);
            _iD = Guid.NewGuid();
            rand = null;
        }


        public string FolderName()
        {
            return _iD.ToString().Substring(0, 4) + "_" + _Randomnumber;
        }

        [DataMember]
        public Guid ID { get { return _iD; } set { _iD = value; } }

        [DataMember]
        public bool IsExistingAttachment { get; set; }

        [DataMember]
        public bool IsLineAttachment { get; set; }

        [DataMember]
        public Byte[] AttachmentData { get; set; }

        [DataMember]
        public string AttachmentsName { get; set; }

        private string _AttachmentsDiskName;

        [DataMember]
        public string AttachmentsDiskName
        {
            get
            {
                //if (string.IsNullOrEmpty(AttachmentsName)) return AttachmentsName;
                //int AttNameLength = AttachmentsName.Length;
                // int totallength = GetApplicationPath() + _iD.ToString().Length + AttNameLength;
                int CheckLength = 50;
                //if (totallength > 140)
                //{
                //    CheckLength = (140 - GetApplicationPath() + _iD.ToString().Length);
                //}
                _AttachmentsDiskName = (AttachmentsName.Length > CheckLength) ? AttachmentsName.Substring(0, CheckLength - 1) : AttachmentsName;
                if (AttachmentsName.Length > CheckLength)
                {
                    string name = AttachmentsName.Substring(AttachmentsName.Length - 10, 10);
                    string Extension = (name.LastIndexOf(".") > 0) ? name.Substring(name.LastIndexOf("."), name.Length - name.LastIndexOf(".")) : "";
                    _AttachmentsDiskName = AttachmentsName.Substring(0, CheckLength - 10) + Extension;
                }
                else
                { _AttachmentsDiskName = AttachmentsName; }
                if (AttachmentsName.EndsWith(".eml"))
                {
                    _AttachmentsDiskName = _AttachmentsDiskName + ".eml";
                }
                else if (AttachmentType == AttachementType.ItemAttachment)
                {
                    _AttachmentsDiskName = _AttachmentsDiskName + ".eml";
                }

                return _AttachmentsDiskName;
            }

            set { value = _AttachmentsDiskName; }
        }

        [DataMember]
        public AttachementType AttachmentType { get; set; }

        [DataMember]
        public string FileFullPath { get; set; }

        [DataMember]
        public string ContentId { get; set; }
        [DataMember]
        public string ContentType { get; set; }
        [DataMember]
        public bool isDeleted { get; set; }

        [DataMember]
        public string MailMessageID { get; set; }



        private int GetApplicationPath()
        {
            string mainFoldername = "EXL_AppData";
            string foldername = "Email";

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //#if (!DEBUG)
            //                codeBase = codeBase.Substring(0, codeBase.IndexOf("Local")); 
            //#endif
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string directory = Path.GetDirectoryName(path);
            CreateFolder(Path.DirectorySeparatorChar.ToString() + mainFoldername);
            CreateFolder(directory + Path.DirectorySeparatorChar.ToString() + mainFoldername + Path.DirectorySeparatorChar.ToString() + foldername);
            directory = directory + Path.DirectorySeparatorChar.ToString() + mainFoldername + Path.DirectorySeparatorChar.ToString() + foldername;

            return (directory.Length > 0) ? directory.Length : 10;

        }
        private void CreateFolder(string path)
        {
            try
            {
                // If the directory doesn't exist, create it.
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
