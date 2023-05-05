using CMS_WT_API.Dtos;

namespace CMS_DL
{
    public interface IBaseDL<T> where T : class
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi cần lấy</returns>
        Task<List<T>> GetAllRecord();
        /// <summary>
        /// Lấy tất cả bản ghi theo phân trang và tìm kiếm
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageIndex">Thứ tự trang muốn lấy</param>
        /// <param name="keyWord">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách bản ghi theo phân trang và tìm kiếm</returns>
        Task<PagedResult<T>> GetRecordPagingAndFilter(int pageSize, int pageIndex, string keyWord);
        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="RecordID">ID bản ghi muốn lấy</param>
        /// <returns>Bản ghi cần lấy</returns>
        Task<T> GetRecordByID(int RecordID);
        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi cần thêm mới</param>
        /// <returns>Bản ghi vừa thêm</returns>
        Task<int> CreateRecord(T record);
        /// <summary>
        /// Sửa 1 bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi cần sửa</param>
        /// <param name="recoreId">ID bản ghi cần sửa</param>
        /// <returns>ID bản ghi vừa sửa</returns>
        Task<int> UpdateRecord(T record, int recoreId);
        /// <summary>
        /// Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xóa</param>
        /// <returns>ID bản ghi vừa xóa</returns>
        Task<int> DeleteRecordByID(int recordID);
        /// <summary>
        /// Xóa nhiều bản ghi theo ID
        /// </summary>
        /// <param name="recordIds">Danh sách các ID cần xóa</param>
        /// <returns>Danh sách các ID vừa xóa</returns>
        Task DeleteManyRecord(List<int> recordIds);
    }
}
