using Microsoft.AspNetCore.Mvc.Rendering;

namespace MicUI.NerveHub.Common
{
    public class BaseFilter
    {
        public BaseFilter()
        {
            RowPerPage = CommonSetting.DefaultPageSize;
            page = CommonSetting.DefaultPageNumber;
            sortdir = CommonSetting.DefaultSortDir;
        }
        public string AdvanceSearchText { get; set; }
        public string SearchText { get; set; }
        private DateTime? dateCreated;
        public DateTime AdvanceSearchTextDate
        {
            get { return dateCreated ?? DateTime.Now; }
            set { dateCreated = value; }
        }
        public string ColumnName { get; set; }
        public string sort { get; set; }
        public string sortdir { get; set; }
        public int page { get; set; }
        public int RowPerPage { get; set; }
        public int TotalRows { get; set; }
        public int PageCount
        {
            get
            {
                int count = 0;
                if (TotalRows != 0)
                {

                    count = TotalRows / RowPerPage;
                    if (TotalRows % RowPerPage > 1)
                        count++;
                }

                return count;
            }
        }
        public List<SelectListItem> ColumnsList { get; set; }

    }
}
