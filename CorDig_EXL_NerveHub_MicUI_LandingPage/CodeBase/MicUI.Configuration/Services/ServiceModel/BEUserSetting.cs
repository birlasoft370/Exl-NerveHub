using System.Runtime.Serialization;

namespace MicUI.Configuration.Services.ServiceModel
{
    [Serializable]
    // [DataContract]
    public class BEUserSetting : ObjectBase
    {
        /// <summary>
        /// Gets or sets the process ID.
        /// </summary>
        /// <value>The process ID.</value>
        [DataMember]
        public int ProcessID { get; set; }

        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        /// <value>The name of the process.</value>
        [DataMember]
        public string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the ERP process ID.
        /// </summary>
        /// <value>The ERP process ID.</value>
        [DataMember]
        public int ERPProcessID { get; set; }

        /// <summary>
        /// Gets or sets the name of the ERP process.
        /// </summary>
        /// <value>The name of the ERP process.</value>
        [DataMember]
        public string ERPProcessName { get; set; }

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        /// <value>The role ID.</value>
        [DataMember]
        public int RoleID { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public bool IsClientRole { get; set; }

        /// <summary>
        /// Gets or sets the job ID.
        /// </summary>
        /// <value>The job ID.</value>
        [DataMember]
        public int JobID { get; set; }

        /// <summary>
        /// Gets or sets the job desc.
        /// </summary>
        /// <value>The job desc.</value>
        [DataMember]
        public string JobDesc { get; set; }

        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        [DataMember]
        public string LoginName { get; set; }

        /// <summary>
        /// Mapped On
        /// Client =1
        /// Process=2
        /// Campaign =3
        /// </summary>
        /// <value>User Mapped On.</value>
        [DataMember]
        public int MappedOn { get; set; }

    }
}
