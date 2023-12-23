namespace MicSer.Security.UserMgt.Models
{
    public class Module_Data
    {
        public string ModuleName { get; set; }
        public int DisplayOrder { get; set; }

        public string ModuleAction { get; set; }

        public bool Module_Action { get; set; }

        public string ModuleText { get; set; }
    }
    public class BELandingPageMenu
    {
        public string TEXT { get; set; }
        public string MODULENAME { get; set; }
        public bool READACTION { get; set; }


    }
    public class MenuItems
    {
       

            public int menuid { get; set; }

            public string text { get; set; }

            public int parentid { get; set; }

            public string url { get; set; }

            public string flag { get; set; }

            // public string TOOLTIP { get; set; }
            public int formId { get; set; }

            public int displayorder { get; set; }

            public int formtype { get; set; }

            public bool haspermission { get; set; }

            public string controller { get; set; }

            public string action { get; set; }

            public string iconclass { get; set; }

            public string modulename { get; set; }
            public int nodeid { get; set; }
            public string nodename { get; set; }

        

    }
}
