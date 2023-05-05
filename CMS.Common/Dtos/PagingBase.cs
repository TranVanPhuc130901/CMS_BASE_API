namespace CMS_WT_API.Dtos
{
    public class PagingBase
    {
        /// <summary>
        /// Vi trí(trang số bao nhiêu)
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// Kích cỡ trang
        /// </summary>
        public int PageSize { get; set; }
    }
}
