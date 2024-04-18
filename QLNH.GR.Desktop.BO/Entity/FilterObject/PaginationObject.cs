namespace QLNH.GR.Desktop.BO
{
    /// <summary>
    /// object of pagination
    /// created by Thi Nguyen (20-8-2023)
    /// </summary>
    /// 
    public class PaginationObject
    {
        public List<FilterObject>? FilterObjects { get; set; }

        public List<SortObject>? SortObjects { get; set; }

        public int? RecentPage { get; set; }

        public int? PageSize { get; set; }
    }
}
