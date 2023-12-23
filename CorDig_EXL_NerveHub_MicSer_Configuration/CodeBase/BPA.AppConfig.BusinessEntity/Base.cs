using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity
{
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
