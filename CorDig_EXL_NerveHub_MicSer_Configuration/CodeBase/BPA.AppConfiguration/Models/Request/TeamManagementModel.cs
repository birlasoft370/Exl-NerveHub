namespace BPA.AppConfiguration.Models.Request
{
    public class TeamManagementModel
    {
        public TeamManagementModel()
        {
            UserList = new();
        }
        public int ClientID { get; set; }
        public int ProcessID { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public bool Disable { get; set; }

        public bool IsClientLevelTeam { get; set; }
        public string Description { get; set; }

        public List<User> UserList { get; set; }

        public class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
        }
    }
}
