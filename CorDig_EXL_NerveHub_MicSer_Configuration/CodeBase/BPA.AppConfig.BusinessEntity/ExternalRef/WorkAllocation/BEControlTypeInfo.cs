using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    [DataContract]
    public class BEControlTypeInfo : ObjectBase
    {
        private int _iControlTypeID;
        private string _sControlType;

        /// <summary>
        /// Initializes a new instance of the <see cref="BEControlTypeInfo"/> class.
        /// </summary>
        public BEControlTypeInfo()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEControlTypeInfo"/> class.
        /// </summary>
        /// <param name="iControlTypeID">The i control type ID.</param>
        /// <param name="sControlType">Type of the s control.</param>
        public BEControlTypeInfo(int iControlTypeID, string sControlType)
        {
            _iControlTypeID = iControlTypeID;
            _sControlType = sControlType;
        }

        /// <summary>
        /// Gets or sets the i control type ID.
        /// </summary>
        /// <value>The i control type ID.</value>
        [DataMember]
        public int iControlTypeID
        {
            get
            {
                return _iControlTypeID;
            }
            set
            {
                _iControlTypeID = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the s control.
        /// </summary>
        /// <value>The type of the s control.</value>
        [DataMember]
        public string sControlType
        {
            get
            {
                return _sControlType;
            }
            set
            {
                _sControlType = value;
            }
        }


    }
}
