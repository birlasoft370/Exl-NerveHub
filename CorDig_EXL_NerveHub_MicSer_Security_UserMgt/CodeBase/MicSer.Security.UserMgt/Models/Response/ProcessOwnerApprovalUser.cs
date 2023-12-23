using BPA.Security.BusinessEntity.ExtrernalRefre;

namespace MicSer.Security.UserMgt.Models.Response
{
    public class ProcessOwnerApprovalUser
    {
      
        public List<BEApproval> UserList { get { return _UserList; } set { _UserList = value; } }

        List<BEApproval> _UserList = new ();
        public List<int> UserIdList { get { return _userIdList; } set { _userIdList = value; } }

        List<int> _userIdList = new();
     

    }
}
