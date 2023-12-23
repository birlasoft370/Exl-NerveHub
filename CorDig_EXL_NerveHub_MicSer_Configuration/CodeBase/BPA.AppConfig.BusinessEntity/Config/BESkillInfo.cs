using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    [DataContract]
    public class BESkillInfo : ObjectBase
    {
        private int _iSkillID;
        private string _sSkillName;
        private string _sSkillDescription;


        /// <summary>
        /// Initializes a new instance of the <see cref="oSkillInfo"/> class.
        /// </summary>
        public BESkillInfo()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="oSkillInfo"/> class.
        /// </summary>
        /// <param name="iSkillID">The skill ID.</param>
        /// <param name="sSkillName">Name of the skill.</param>
        /// <param name="sSkillDescription">The skill description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">The created by.</param>
        /// <param name="dCreateDate">The create date.</param>
        public BESkillInfo(int iSkillID, string sSkillName, string sSkillDescription,
            bool bDisabled, int iCreatedBy, DateTime dCreateDate)
        {
            _iSkillID = iSkillID;
            _sSkillName = sSkillName;
            _sSkillDescription = sSkillDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
            base.dCreateDate = dCreateDate;
        }

        /// <summary>
        /// Gets or sets the skill ID.
        /// </summary>
        /// <value>The skill ID.</value>
        [DataMember]
        public int iSkillID
        {
            get { return _iSkillID; }
            set { _iSkillID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the skill.
        /// </summary>
        /// <value>The name of the skill.</value>
        [DataMember]
        public string sSkillName
        {
            get { return _sSkillName; }
            set { _sSkillName = value; }
        }

        /// <summary>
        /// Gets or sets the shift description.
        /// </summary>
        /// <value>The shift description.</value>
        [DataMember]
        public string sSkillDescription
        {
            get { return _sSkillDescription; }
            set { _sSkillDescription = value; }
        }


    }
}
