
using AutoMapper;
using CMS_Common;
using CMS_Common.Database;
using CMS_WT_API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CMS_DL
{
    public class BaseDL<T> : IBaseDL<T> where T : class
    {
        #region filed
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        #endregion
        #region constructor
        public BaseDL(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        #region class
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        public virtual async Task<List<T>> GetAllRecord()
        {
            return await _context.Set<T>().ToListAsync();
        }
        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="RecordID">ID bản ghi cần lấy</param>
        /// <returns>Bản ghi cần lấy</returns>
        /// <exception cref="MyException"></exception>
        public virtual async Task<T> GetRecordByID(int RecordID)
        {
            try
            {
                var record = await _context.Set<T>().FindAsync(RecordID);
                if (record == null)
                {
                    throw new MyException("This is my exception");
                }
                return record;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Lấy bản ghi theo tìm kiếm và phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageIndex">Trang muốn hiển thị</param>
        /// <param name="keyWord">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách bản ghi thỏa mãn</returns>
        /// <exception cref="MyException"></exception>
        public virtual async Task<PagedResult<T>> GetRecordPagingAndFilter(int pageSize, int pageIndex, string keyWord)
        {
            try
            {
                var query = _context.Set<T>().AsQueryable();
                if (!string.IsNullOrEmpty(keyWord))
                {
                    query = query.Where(x => x.ToString().Contains(keyWord));
                }
                else
                {
                    keyWord = "";
                }

                var totalRow = await query.CountAsync();
                if (pageSize == 0)
                {
                    pageSize = 20;
                    pageIndex = 1;
                }
                var records = await query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                return new PagedResult<T>(records, totalRow, pageSize, pageIndex, keyWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi muốn thêm mới</param>
        /// <returns>Thông tin bản ghi vừa thêm</returns>
        /// <exception cref="MyException"></exception>
        public virtual async Task<int> CreateRecord(T record)
        {
            try
            {

                _context.Set<T>().Add(record);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Sửa 1 bản ghi theo ID
        /// </summary>
        /// <param name="record">Thông tin bản ghi muốn sửa</param>
        /// <param name="recordId">ID bản ghi muốn sửa</param>
        /// <returns>ID bản ghi vừa sửa</returns>
        /// <exception cref="MyException"></exception>
        public virtual async Task<int> UpdateRecord(T record, int recordId)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(recordId);
                if (entity == null)
                {
                    throw new Exception($"Record with id {recordId} not found.");
                }
                _context.Entry(entity).CurrentValues.SetValues(record);
                _context.Update(entity);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi muốn xóa</param>
        /// <returns>ID bản ghi vừa xóa</returns>
        /// <exception cref="MyException"></exception>
        public async Task<int> DeleteRecordByID(int recordID)
        {
            try
            {
                var record = await _context.Set<T>().FindAsync(recordID);
                if (record == null)
                {
                    throw new MyException("This is my recordId not fround");
                }
                _context.Set<T>().Remove(record);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Xóa nhiều bản ghi theo ID
        /// </summary>
        /// <param name="recordIds">Danh sách bản ghi muốn xóa</param>
        /// <returns>Danh sách ID vừa xóa</returns>
        /// <exception cref="MyException"></exception>
        public async Task DeleteManyRecord(List<int> recordIds)
        {
            try
            {
                if (recordIds == null || !recordIds.Any())
                    return;

                foreach (var id in recordIds)
                {
                    var record = await _context.Set<T>().FindAsync(id);
                    if (record != null)
                        _context.Set<T>().Remove(record);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        } 
        #endregion

    }
}