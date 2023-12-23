using BPA.EmailManagement.BusinessEntity.ExternalRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessEntity
{
    [Serializable]
    [DataContract]
    public class BEMailTemplate : ObjectBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        /// <value>The client ID.</value>
        [DataMember]
        public int iCampaignID { get; set; }

        /// <summary>
        /// Gets or sets the process ID.
        /// </summary>
        /// <value>process ID.</value>
        [DataMember]
        public int iProcessID { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        /// <value>The name of the client.</value>
        [DataMember]
        public string sCampaignName { get; set; }


        /// <summary>
        /// Gets or sets the i client ID.
        /// </summary>
        /// <value>The i client ID.</value>
        [DataMember]
        public int iClientID { get; set; }

        /// <summary>
        /// Gets or sets the i Mail Tempalte ID.
        /// </summary>
        /// <value>The i Mail Tempalte ID.</value>
        [DataMember]
        public int iMailTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Mail Template.
        /// </summary>
        /// <value>The name of the Mail Template.</value>
        [DataMember]
        public string sMailTemplateName { get; set; }

        /// <summary>
        /// Gets or sets the  Mail Template.
        /// </summary>
        /// <value>The Mail Template.</value>
        [DataMember]
        public string sMailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the Auto Replay.
        /// </summary>
        /// <value>The Auto Replay boolean.</value>
        [DataMember]
        public bool bIsAutoReplay { get; set; }

        [DataMember]
        public List<MailTemplateImage> lstImgMailTemp { get; set; }

        #endregion
    }

    [Serializable]
    [DataContract]
    public class MailTemplateImage
    {
        [DataMember]
        public int iTempImageID { get; set; }
        [DataMember]
        public int iMailImageTemplateId { get; set; }
        [DataMember]
        public string sImageName { get; set; }
        [DataMember]
        public string sImageWidth { get; set; }
        [DataMember]
        public string sImageHeight { get; set; }
        [DataMember]
        public byte[] bImageMail { get; set; }
    }
}
