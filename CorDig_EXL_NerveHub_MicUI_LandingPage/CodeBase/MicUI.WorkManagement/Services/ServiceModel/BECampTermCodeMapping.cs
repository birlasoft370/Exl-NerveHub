using System.Data;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    public class BECampTermCodeMapping : ObjectBase
    {
        private int _iCampaignID;
        private DataTable _dtTerminationTable;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BECampTermCodeMapping()
        { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="iCampaignID">The campaign ID.</param>
        public BECampTermCodeMapping(int iCampaignID)
        {
            _iCampaignID = iCampaignID;
        }

        /// <summary>
        /// Gets or sets the i campaign ID.
        /// </summary>
        /// <value>The i campaign ID.</value>
        public int iCampaignID
        {
            get { return _iCampaignID; }
            set { _iCampaignID = value; }

        }


        /// <summary>
        /// Gets or sets the dt termination table.
        /// </summary>
        /// <value>The dt termination table.</value>
        public DataTable dtTerminationTable
        {
            get { return _dtTerminationTable; }
            set { _dtTerminationTable = value; }
        }

    }
}
