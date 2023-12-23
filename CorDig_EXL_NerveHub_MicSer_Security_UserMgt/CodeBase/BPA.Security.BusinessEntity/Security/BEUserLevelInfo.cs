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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace BPA.Security.BusinessEntity.Security
{
    /// <summary>
    ///  Used to transfer data for User Level object between the layers
    /// </summary>
    [Serializable]
    [DataContract]
    public class BEUserLevelInfo:ObjectBase
    {
        #region Fields
        private int _iUserLevelID;
        private string _sLevelName;
        private string _sDescription;
        private int _iParentID;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BEUserLevelInfo"/> class.
        /// </summary>
        public BEUserLevelInfo()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BEUserLevelInfo"/> class.
        /// </summary>
        /// <param name="iUserLevelID">The user level ID.</param>
        /// <param name="sLevelName">Name of the level.</param>
        /// <param name="sDescription">The description.</param>
        /// <param name="iParentID">The parent ID.</param>
        /// <param name="bDisabled">if set to <c>true</c> [ disabled].</param>
        /// <param name="iCreatedBy">The  created by.</param>
        public BEUserLevelInfo(int iUserLevelID, string sLevelName, string sDescription,int iParentID, bool bDisabled, int iCreatedBy)
        {
            _iUserLevelID = iUserLevelID;
            _sLevelName= sLevelName;
            _sDescription= sDescription;
            _iParentID = iParentID;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the user level ID.
        /// </summary>
        /// <value>user level ID.</value>
        [DataMember]
        public int iUserLevelID
        {
            get { return _iUserLevelID; }
            set { _iUserLevelID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the level.
        /// </summary>
        /// <value>The name of the level.</value>
        [DataMember]
        public string sLevelName
        {
            get { return _sLevelName; }
            set { _sLevelName = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        public string sDescription
        {
            get { return _sDescription; }
            set { _sDescription = value; }
        }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        /// <value>The parent ID.</value>
        [DataMember]
        public int iParentID
        {
            get { return _iParentID; }
            set { _iParentID = value; }
        }
        #endregion
    }
}
