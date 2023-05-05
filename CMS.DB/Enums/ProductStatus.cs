namespace CMS_WT_API.Enums
{
    public enum ProductStatus
    {
        /// <summary>
        /// Trạng thái chưa kích hoạt
        /// </summary>
        UnActive = 0,
        /// <summary>
        /// Trạng thái đã kích hoạt
        /// </summary>
        Active = 1,
        /// <summary>
        /// Trạng thái trong thùng rác
        /// </summary>
        Deleted = 2,
        /// <summary>
        /// Trạng thái chờ xử lý
        /// </summary>
        Pending = 3,
    }
}
