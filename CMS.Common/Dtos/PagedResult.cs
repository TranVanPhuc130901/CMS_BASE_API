namespace CMS_WT_API.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalRow { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public string KeyWord { get; set; }

        public PagedResult(List<T> items, int totalRow, int pageSize, int pageIndex, string keyWord)
        {
            Items = items;
            TotalRow = totalRow;
            PageSize = pageSize;
            PageIndex = pageIndex;
            KeyWord = keyWord;
        }

    }
}
