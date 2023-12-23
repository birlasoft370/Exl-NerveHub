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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BPA.Security.BusinessEntity
{
    /// <summary>
    /// This Class is used by all business entity as Base class.
    /// This class is having some common properties used by all the business Entity.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class ObjectBase : IDisposable
    {
        private bool _bFreshTransaction;
        private bool _bDisabled;
        private int _iCreatedBy;
        private DateTime _dCreateDate;
        private int _iModifiedBy;
        private DateTime _dModifyDate;
        private bool _bIsBot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBase"/> class.
        /// </summary>
        public ObjectBase()
        { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        /// <summary>
        /// Gets or sets a value indicating whether [disabled].
        /// </summary>
        /// <value><c>true</c> if [disabled]; otherwise, <c>false</c>.</value>
        /// 
        [DataMember]
        public bool bDisabled
        {
            get { return _bDisabled; }
            set { _bDisabled = value; }
        }

        [DataMember]
        public bool bIsBot
        {
            get { return _bIsBot; }
            set { _bIsBot = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [FreshTransaction].
        /// </summary>
        /// <value><c>true</c> if [FreshTransaction]; otherwise, <c>false</c>.</value>
        /// 
        [DataMember]
        public bool bFreshTransaction
        {
            get { return _bFreshTransaction; }
            set { _bFreshTransaction = value; }
        }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        /// 
        [DataMember]
        public int iCreatedBy
        {
            get { return _iCreatedBy; }
            set { _iCreatedBy = value; }
        }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        /// 
        [DataMember]
        public DateTime dCreateDate
        {
            get { return _dCreateDate; }
            set { _dCreateDate = value; }
        }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>The modified by.</value>
        /// 
        [DataMember]
        public int iModifiedBy
        {
            get { return _iModifiedBy; }
            set { _iModifiedBy = value; }
        }

        /// <summary>
        /// Gets or sets the modify date.
        /// </summary>
        /// <value>The modify date.</value>
        /// 

        [DataMember]
        public DateTime dModifyDate
        {
            get { return _dModifyDate; }
            set { _dModifyDate = value; }
        }
    }
      
}
