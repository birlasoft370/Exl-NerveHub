using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Models.ViewModels
{
    [Serializable]
    public class PowerUserViewModel
    {

        public int UserId { get; set; }

        public string LoginName { get; set; }

        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public bool? Disabled { get; set; }

        public int Role { get; set; }

        public int JobId { get; set; }

        public int Approver { get; set; }

        public string UserName { get; set; }

        public bool LanId { get; set; }

        public int PendingApproval { get; set; }

        [Serializable]
        public class clsApprover
        {
            public int UserId { get; set; }

            [ScaffoldColumn(false)]
            public string EncryptUserId { get; set; }
            public string ApproverName { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string LanId { get; set; }
            public int EmployeeId { get; set; }
            public string Email { get; set; }
            public bool IsDisabled { get; set; }

        }




        public IEnumerable<clsApprover> ApproverList = new List<clsApprover>();

    }
}
