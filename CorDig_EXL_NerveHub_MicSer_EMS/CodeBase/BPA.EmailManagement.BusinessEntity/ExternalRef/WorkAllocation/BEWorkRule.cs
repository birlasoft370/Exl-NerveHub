using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    public class BEWorkRule : ObjectBase
    {
        private int _iWorkRuleId;

        private int _iTDSObjId1;
        private string _sTDSObjId1Prefix;
        private int _iTDSObjId2;
        private string _sTDSObjId2Prefix;
        private int _iTDSObjId3;
        private string _sTDSObjId3Prefix;
        private int _iActionId;
        private int _iActionOn;
        private string _sActionOnValue;
        private int _iRuleTypeId;
        private int _iEventObjectId;
        private string _sEventObjectIdPrefix;
        private string _sDefaultValue;
        private bool _bFormula;
        private DataTable _dtCondition;
        private string _sConditionExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="BEWorkRule"/> class.
        /// </summary>
        public BEWorkRule()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEWorkRule"/> class.
        /// </summary>
        /// <param name="iWorkRuleId">The i work rule id.</param>
        /// <param name="iTDSObjId1">The i TDS obj id1.</param>
        /// <param name="sTDSObjId1Prefix">The s TDS obj id1 prefix.</param>
        /// <param name="iTDSObjId2">The i TDS obj id2.</param>
        /// <param name="sTDSObjId2Prefix">The s TDS obj id2 prefix.</param>
        /// <param name="iTDSObjId3">The i TDS obj id3.</param>
        /// <param name="sTDSObjId3Prefix">The s TDS obj id3 prefix.</param>
        /// <param name="iActionId">The i action id.</param>
        /// <param name="iActionOn">The i action on.</param>
        /// <param name="sActionOnValue">The s action on value.</param>
        /// <param name="iRuleTypeId">The i rule type id.</param>
        /// <param name="iEventObjectId">The i event object id.</param>
        /// <param name="sEventObjectIdPrefix">The s event object id prefix.</param>
        /// <param name="sDefaultValue">The s default value.</param>
        /// <param name="bFormula">if set to <c>true</c> [b formula].</param>
        /// <param name="sConditionExpression">The s condition expression.</param>
        public BEWorkRule(int iWorkRuleId, int iTDSObjId1,
            string sTDSObjId1Prefix, int iTDSObjId2,
            string sTDSObjId2Prefix, int iTDSObjId3,
            string sTDSObjId3Prefix, int iActionId, int iActionOn, string sActionOnValue, int iRuleTypeId,
            int iEventObjectId, string sEventObjectIdPrefix, string sDefaultValue, bool bFormula, string sConditionExpression)
        {
            _iWorkRuleId = iWorkRuleId;
            _iTDSObjId1 = iTDSObjId1;
            _sTDSObjId1Prefix = sTDSObjId1Prefix;
            _iTDSObjId2 = iTDSObjId2;
            _sTDSObjId2Prefix = sTDSObjId2Prefix;
            _iTDSObjId3 = iTDSObjId3;
            _sTDSObjId3Prefix = sTDSObjId3Prefix;
            _iActionId = iActionId;
            _sActionOnValue = sActionOnValue;
            _iActionOn = iActionOn;
            _iRuleTypeId = iRuleTypeId;
            _iEventObjectId = iEventObjectId;
            _sEventObjectIdPrefix = sEventObjectIdPrefix;
            _sDefaultValue = sDefaultValue;
            _bFormula = bFormula;
            _sConditionExpression = sConditionExpression;

            base.iCreatedBy = iCreatedBy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEWorkRule"/> class.
        /// </summary>
        /// <param name="iWorkRuleId">The i work rule id.</param>
        /// <param name="iTDSObjId1">The i TDS obj id1.</param>
        /// <param name="iTDSObjId2">The i TDS obj id2.</param>
        /// <param name="iTDSObjId3">The i TDS obj id3.</param>
        /// <param name="iActionId">The i action id.</param>
        /// <param name="iActionOn">The i action on.</param>
        /// <param name="iRuleTypeId">The i rule type id.</param>
        /// <param name="iEventObjectId">The i event object id.</param>
        /// <param name="sDefaultValue">The s default value.</param>
        /// <param name="bFormula">if set to <c>true</c> [b formula].</param>
        /// <param name="dtCondition">The dt condition.</param>
        public BEWorkRule(int iWorkRuleId, int iTDSObjId1, int iTDSObjId2, int iTDSObjId3, int iActionId, int iActionOn, int iRuleTypeId, int iEventObjectId, string sDefaultValue, bool bFormula, DataTable dtCondition)
        {
            _iWorkRuleId = iWorkRuleId;
            _iTDSObjId1 = iTDSObjId1;
            _iTDSObjId2 = iTDSObjId2;
            _iTDSObjId3 = iTDSObjId3;
            _iActionId = iActionId;
            _iActionOn = iActionOn;
            _iRuleTypeId = iRuleTypeId;
            _iEventObjectId = iEventObjectId;
            _sDefaultValue = sDefaultValue;
            _bFormula = bFormula;
            _dtCondition = dtCondition;
            base.iCreatedBy = iCreatedBy;
        }


        /// <summary>
        /// Gets or sets the dt condition.
        /// </summary>
        /// <value>The dt condition.</value>
        public DataTable dtCondition
        {
            get { return _dtCondition; }
            set { _dtCondition = value; }
        }


        /// <summary>
        /// Gets or sets the s default value.
        /// </summary>
        /// <value>The s default value.</value>
        public string sDefaultValue
        {
            get { return _sDefaultValue; }
            set { _sDefaultValue = value; }
        }

        public string sConditionExpression
        {
            get { return _sConditionExpression; }
            set { _sConditionExpression = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [b formula].
        /// </summary>
        /// <value><c>true</c> if [b formula]; otherwise, <c>false</c>.</value>
        public bool bFormula
        {
            get { return _bFormula; }
            set { _bFormula = value; }
        }

        /// <summary>
        /// Gets or sets the i event object id.
        /// </summary>
        /// <value>The i event object id.</value>
        public int iEventObjectId
        {
            get { return _iEventObjectId; }
            set { _iEventObjectId = value; }
        }

        /// <summary>
        /// Gets or sets the s event object id prefix.
        /// </summary>
        /// <value>The s event object id prefix.</value>
        public string sEventObjectIdPrefix
        {
            get { return _sEventObjectIdPrefix; }
            set { _sEventObjectIdPrefix = value; }
        }
        /// <summary>
        /// Gets or sets the i rule type id.
        /// </summary>
        /// <value>The i rule type id.</value>
        public int iRuleTypeId
        {
            get { return _iRuleTypeId; }
            set { _iRuleTypeId = value; }
        }

        /// <summary>
        /// Gets or sets the i work rule id.
        /// </summary>
        /// <value>The i work rule id.</value>
        public int iWorkRuleId
        {
            get { return _iWorkRuleId; }
            set { _iWorkRuleId = value; }
        }


        /// <summary>
        /// Gets or sets the i TDS obj id1.
        /// </summary>
        /// <value>The i TDS obj id1.</value>
        public int iTDSObjId1
        {
            get { return _iTDSObjId1; }
            set { _iTDSObjId1 = value; }
        }

        /// <summary>
        /// Gets or sets the i TDS obj id1 prefix.
        /// </summary>
        /// <value>The i TDS obj id1 prefix.</value>
        public string sTDSObjId1Prefix
        {
            get { return _sTDSObjId1Prefix; }
            set { _sTDSObjId1Prefix = value; }
        }

        /// <summary>
        /// Gets or sets the i TDS obj id2.
        /// </summary>
        /// <value>The i TDS obj id2.</value>
        public int iTDSObjId2
        {
            get { return _iTDSObjId2; }
            set { _iTDSObjId2 = value; }
        }


        /// <summary>
        /// Gets or sets the s TDS obj id2 prefix.
        /// </summary>
        /// <value>The s TDS obj id2 prefix.</value>
        public string sTDSObjId2Prefix
        {
            get { return _sTDSObjId2Prefix; }
            set { _sTDSObjId2Prefix = value; }
        }

        /// <summary>
        /// Gets or sets the i TDS obj id3.
        /// </summary>
        /// <value>The i TDS obj id3.</value>
        public int iTDSObjId3
        {
            get { return _iTDSObjId3; }
            set { _iTDSObjId3 = value; }
        }

        /// <summary>
        /// Gets or sets the s TDS obj id3 prefix.
        /// </summary>
        /// <value>The s TDS obj id3 prefix.</value>
        public string sTDSObjId3Prefix
        {
            get { return _sTDSObjId3Prefix; }
            set { _sTDSObjId3Prefix = value; }
        }
        /// <summary>
        /// Gets or sets the i action id.
        /// </summary>
        /// <value>The i action id.</value>
        public int iActionId
        {
            get { return _iActionId; }
            set { _iActionId = value; }
        }

        /// <summary>
        /// Gets or sets the i action on.
        /// </summary>
        /// <value>The i action on.</value>
        public int iActionOn
        {
            get { return _iActionOn; }
            set { _iActionOn = value; }
        }

        /// <summary>
        /// Gets or sets the i action on.
        /// </summary>
        /// <value>The i action on.</value>
        public string sActionOnValue
        {
            get { return _sActionOnValue; }
            set { _sActionOnValue = value; }
        }

    }
}
