/* Copyright © 2012 ExlService (I) Pvt. Ltd.
 * project Name                 :   
 * Class Name                   :   
 * Namespace                    :   
 * Purpose                      :
 * Description                  :
 * Dependency                   :   
 * Related Table                :
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :   
 * Created on                   :   
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */


using BPA.Security.BusinessEntity.ExtrernalRefre;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BPA.Security.BusinessEntity.Security
{
    [Serializable]
    [DataContract]
    public class BEUserMapping : ObjectBase
    {
         private DateTime _dtEffectiveDate;
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (oRole != null)
                oRole.Dispose();
            if (oUser != null)
                oUser.Dispose();
            if (oClient != null)
                oClient = null;
            if (oProcess != null)
                oProcess = null;
            if (oCampaign != null)
                oCampaign = null;
            base.Dispose();

        }

        /// <summary>
        /// Gets or sets the i user process map ID.
        /// </summary>
        /// <value>The i user process map ID.</value>
        [DataMember]
        public int iUserProcessMapID { get; set; }
        /// <summary>
        /// Gets or sets the object role.
        /// </summary>
        /// <value>The object role.</value>
        [DataMember]
        public BERoleInfo oRole { get; set; }

        /// <summary>
        /// Gets or sets the object user.
        /// </summary>
        /// <value>The object user.</value>
        [DataMember]
        public BEUserInfo oUser { get; set; }

        /// <summary>
        /// Gets or sets the object client.
        /// </summary>
        /// <value>The object client.</value>
        [DataMember]
        public IList<BEClientInfo> oClient { get; set; }

        /// <summary>
        /// Gets or sets the object process.
        /// </summary>
        /// <value>The object process.</value>
        [DataMember]
        public IList<BEProcessInfo> oProcess { get; set; }

        /// <summary>
        /// Gets or sets the object campaign.
        /// </summary>
        /// <value>The object campaign.</value>
        [DataMember]
        public IList<BECampaignInfo> oCampaign { get; set; }

        /// <summary>
        /// Gets or sets the mapped on.
        /// </summary>
        /// <value>The mapped on.</value>
        [DataMember]
        public int MappedOn { get; set; }

        /// <summary>
        /// Gets or sets the dt effective date.
        /// </summary>
        /// <value>The dt effective date.</value>
        [DataMember]
        public DateTime dtEffectiveDate
        {
            get { return _dtEffectiveDate; }
            set { _dtEffectiveDate = value; }
        }
    }
}
