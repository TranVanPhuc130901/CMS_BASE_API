using CMS_Common.Dtos;
using CMS_WT_API.Dtos;

namespace CMS_BL
{
    public interface IBaseBL<T, TResult> where T : class where TResult : class
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        Task<List<TResult>> GetAllRecord();
        /// <summary>
        /// Lấy tất cả bản ghi theo ID
        /// </summary>
        /// <param name="recordId">ID bản ghi cần lấy</param>
        /// <returns>Bản ghi muốn lấy</returns>
        Task<TResult> GetRecordbyID(int recordId);
        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi cần thêm</param>
        /// <returns>Thông tin bản ghi vừa thêm</returns>
        Task<ServicesResult> CreateRecord(TResult record);
        /// <summary>
        /// Sửa 1 bản ghi theo ID
        /// </summary>
        /// <param name="record">Thông tin bản ghi vừa sửa</param>
        /// <param name="recordId">ID bản ghi muốn sửa</param>
        /// <returns>ID bản ghi vừa sửa</returns>
        Task<ServicesResult> UpdateRecord(TResult record, int recordId);
        /// <summary>
        /// xóa bản ghi theo Id
        /// </summary>
        /// <param name="recordId">Id bản ghi muốn xóa</param>
        /// <returns>Id bản ghi vừa xóa</returns>
        Task<int> DeleteRecordById(int recordId);
        /// <summary>
        /// Lấy bản ghi theo phân trang và tìm kiếm
        /// </summary>
        /// <param name="pageSize">Sô bản ghi trên 1 trang</param>
        /// <param name="pageIndex">Trang muốn hiển thị</param>
        /// <param name="keyWord">Từ khóa cân tìm kiếm</param>
        /// <returns>Danh sách bản ghi thoa mãn</returns>
        Task<PagedResult<TResult>> GetRecordPagingAndFilter(int pageSize, int pageIndex, string keyWord);
        /// <summary>
        /// Xóa nhiều bản ghi theo ID
        /// </summary>
        /// <param name="recordIds">Danh sách bản ghi muốn xóa</param>
        /// <returns>danh sách Id bản ghi vừa xóa</returns>
        Task DeleteManyRecord(List<int> recordIds);
    }

}


