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
    /// Used to transfer data for Action object between the layers 
    /// </summary>
    [Serializable]
    [DataContract]
    public class BEActionInfo:ObjectBase
    {
        #region Fields
        private int _iActionID;
        private string _sActionName;
        private string _sActionDescription;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BEActionInfo"/> class.
        /// </summary>
        public BEActionInfo()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BEActionInfo"/> class.
        /// </summary>
        /// <param name="iActionID">The i action ID.</param>
        /// <param name="sActionName">Name of the s action.</param>
        /// <param name="sActionDescription">The s action description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [b disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BEActionInfo(int iActionID, string sActionName, string sActionDescription, bool bDisabled, int iCreatedBy)
        {
            _iActionID = iActionID;
            _sActionName = sActionName;
            _sActionDescription = sActionDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the action ID.
        /// </summary>
        /// <value>action ID.</value>
        [DataMember]
        public int iActionID
        {
            get { return _iActionID; }
            set { _iActionID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>name of the action.</value>
        [DataMember]
        public string sActionName
        {
            get { return _sActionName; }
            set { _sActionName = value; }
        }
        /// <summary>
        /// Gets or sets the action description.
        /// </summary>
        /// <value>action description.</value>
        [DataMember]
        public string sActionDescription
        {
            get { return _sActionDescription; }
            set { _sActionDescription = value; }
        }
        
        #endregion
    }
}
