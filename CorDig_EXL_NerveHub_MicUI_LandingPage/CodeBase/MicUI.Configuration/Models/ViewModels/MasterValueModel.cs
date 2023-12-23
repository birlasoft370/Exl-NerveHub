namespace MicUI.Configuration.Models.ViewModels
{
    public class MasterValueModel
    {
        public MasterValueModel()
        {
            ValueList = new();
        }
        public int MasterValueID
        {
            get;
            set;
        }
        public string MasterType
        {
            get;
            set;
        }
        public bool? Disabled
        {
            get;
            set;
        }


        public List<MasterValueLists> ValueList { get; set; }
        public bool ShouldSerializeValueList()
        {
            if (ValueList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public class MasterValueLists
    {
        public int FieldID
        {
            get;
            set;
        }
        public string Values
        {
            get;
            set;
        }
        public bool? Disabled
        {
            get;
            set;
        }
    }
}
