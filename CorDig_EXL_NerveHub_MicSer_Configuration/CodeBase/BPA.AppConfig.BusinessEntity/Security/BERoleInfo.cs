using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Security
{
    [Serializable]
    [DataContract]
    public class BERoleInfo : ObjectBase
    {
        private int _iRoleID;
        private string _sRoleName;
        private string _sRoleDescription;
        private int _iLevelID;
        private IList<BEFormAction> _oFormAction;
        private DataTable _dtFormData;



        /// <summary>
        /// Initializes a new instance of the <see cref="BERoleInfo"/> class.
        /// </summary>
        public BERoleInfo()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BERoleInfo"/> class.
        /// </summary>
        /// <param name="iRoleID">The role ID.</param>
        /// <param name="sRoleName">Name of the role.</param>
        /// <param name="sRoleDescription">The role description.</param>
        /// <param name="iCampaignID">The i campaign ID.</param>
        /// <param name="iLevelID">The i level ID.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">The created by.</param>
        public BERoleInfo(int iRoleID, string sRoleName, string sRoleDescription, int iLevelID, bool bDisabled, int iCreatedBy)
        {
            _iRoleID = iRoleID;
            _sRoleName = sRoleName;
            _sRoleDescription = sRoleDescription;
            _iLevelID = iLevelID;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        /// <value>The role ID.</value>
        [DataMember]
        public int iRoleID
        {
            get { return _iRoleID; }
            set { _iRoleID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        [DataMember]
        public string sRoleName
        {
            get { return _sRoleName; }
            set { _sRoleName = value; }
        }

        /// <summary>
        /// Gets or sets the role description.
        /// </summary>
        /// <value>The role description.</value>
        [DataMember]
        public string sRoleDescription
        {
            get { return _sRoleDescription; }
            set { _sRoleDescription = value; }
        }


        /// <summary>
        /// Gets or sets the level ID.
        /// </summary>
        /// <value>The level ID.</value>
        [DataMember]
        public int iLevelID
        {
            get { return _iLevelID; }
            set { _iLevelID = value; }
        }
        /// <summary>
        /// Gets or sets the o form action.
        /// </summary>
        /// <value>The o form action.</value>
        [DataMember]
        public IList<BEFormAction> oFormAction
        {
            get { return _oFormAction; }
            set { _oFormAction = value; }
        }

        /// <summary>
        /// Gets or sets the ds form data.
        /// </summary>
        /// <value>The ds form data.</value>
        [DataMember]
        public DataTable dtFormData
        {
            get { return _dtFormData; }
            set { _dtFormData = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b is client role].
        /// </summary>
        /// <value><c>true</c> if [b is client role]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bIsClientRole
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set Security Group
        /// </summary>
        [DataMember]
        public int iSecurityGroup
        {
            get;
            set;
        }
        //[DataMember]
        //public int iRoleLevel
        //{
        //    get;
        //    set;
        //}
    }
}
