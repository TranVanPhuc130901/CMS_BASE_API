using AutoMapper;
using CMS_Common;
using CMS_Common.CustomAttribute;
using CMS_Common.Dtos;
using CMS_Common.Enums;
using CMS_DL;
using CMS_WT_API.Dtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Azure.Core.HttpHeader;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CMS_BL
{
    public class BaseBL<T, TResult> : IBaseBL<T, TResult> where T : class where TResult : class
    {
        #region filed
        private readonly IBaseDL<T> _baseDL;
        private readonly IMapper _mapper;
        #endregion
        #region constructor
        public BaseBL(IBaseDL<T> baseDL, IMapper mapper)
        {
            _baseDL = baseDL;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, TResult>();
                cfg.CreateMap<TResult, T>();

            });

            _mapper = config.CreateMapper();
        }
        #endregion
        #region class
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        public async virtual Task<List<TResult>> GetAllRecord()
        {
            try
            {
                var records = await _baseDL.GetAllRecord();

                return _mapper.Map<List<TResult>>(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<TResult>();
            }
        }
        /// <summary>
        /// Lấy tất cả bản ghi theo ID
        /// </summary>
        /// <param name="recordId">ID bản ghi muốn lấy</param>
        /// <returns>Bản ghi muốn lấy</returns>
        /// <exception cref="MyException"></exception>
        public virtual async Task<TResult> GetRecordbyID(int recordId)
        {
            try
            {
                var records = await _baseDL.GetRecordByID(recordId);

                return _mapper.Map<TResult>(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Lấy bản ghi theo phân trang và tìm kiếm
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">Trang đang hiển thị</param>
        /// <param name="keyWord">Từ khóa muốn tìm</param>
        /// <returns>Danh sách bản ghi</returns>
        /// <exception cref="MyException"></exception>
        public virtual async Task<PagedResult<TResult>> GetRecordPagingAndFilter(int pageSize, int pageIndex, string keyWord)
        {
            try
            {
                var records = await _baseDL.GetRecordPagingAndFilter(pageSize, pageIndex, keyWord);
                var mappedRecords = _mapper.Map<List<TResult>>(records.Items);
                return new PagedResult<TResult>(mappedRecords, records.TotalRow, records.PageSize, records.PageIndex, records.KeyWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi thêm mới</param>
        /// <returns>Bản ghi vừa thêm mới</returns>
        /// <exception cref="MyException"></exception>
        public virtual async  Task<ServicesResult> CreateRecord(TResult record)
        {
            try
            {
                var validateRequiredData = ValidateRequiredData(record);
                if (!validateRequiredData.isSuccess)
                {
                    return validateRequiredData;
                }
                var mappedRecord = _mapper.Map<T>(record);
                await _baseDL.CreateRecord(mappedRecord);
                return new ServicesResult
                {
                    isSuccess = true
                };
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
        /// <param name="record">Thông tin bản ghi cần sửa</param>
        /// <param name="recordId">ID bản ghi vừa sửa</param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public virtual async Task<int> UpdateRecord(TResult record, int recordId)
        {
            try
            {
                var mappedRecord = _mapper.Map<T>(record);
                return await _baseDL.UpdateRecord(mappedRecord, recordId);
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
        /// <param name="recordId">ID bản ghi muốn xóa</param>
        /// <returns>Id bản ghi vừa xóa</returns>
        /// <exception cref="MyException"></exception>
        public async Task<int> DeleteRecordById(int recordId)
        {
            try
            {
                var records = await _baseDL.DeleteRecordByID(recordId);

                return recordId;
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
        /// <param name="recordIds">Danh sách ID bản ghi muốn xóa</param>
        /// <returns>Danh sách ID bản ghi vừa xóa</returns>
        /// <exception cref="MyException"></exception>
        public async Task DeleteManyRecord(List<int> recordIds)
        {
            try
            {
                await _baseDL.DeleteManyRecord(recordIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        #endregion
        #region validate
        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="record"></param>
        /// <returns>trạng thái bản ghi</returns>
        public virtual ServicesResult ValidateRequiredData(TResult record)
        {
            var className = typeof(TResult).Name;
            // Lấy toàn bộ property của TResult 
            var properties = typeof(TResult).GetProperties();
            // tạo một mảng để lưu các giá trị lỗi
            var validateFail = new List<string>();
            var result = new ServicesResult
            {
                isSuccess = true,
            };

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);
                // validate các trường Maxlength
                var maxLengthAttribute = (MyMaxLengthAttribute?)property.GetCustomAttributes(typeof(MyMaxLengthAttribute), false).FirstOrDefault();
                if (maxLengthAttribute != null && propertyValue != null)
                {
                    var length = ((string)propertyValue).Length;
                    if (length > maxLengthAttribute.MaxLength)
                    {
                        result.isSuccess = false;
                        result.ErrorCode = maxLengthAttribute.ErrorCode;
                        return result;
                    }
                }
                // Validate trường băt buộc
                var requiredAttribute = (RequiredAttribute?)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

                // Nếu properti có attribute là required thì kiểm tra xem giá trị truyền lên có null không 
                if (requiredAttribute != null && (string.IsNullOrEmpty(propertyValue?.ToString()) || propertyValue.ToString() == "00000000-0000-0000-0000-000000000000"))
                {
                    validateFail.Add(propertyName);
                }
            }

            if (validateFail.Count > 0)
            {
                result.isSuccess = false;
                result.ErrorCode = ErrorCode.RequiredValueIsEmpty;
                result.Error = validateFail;
            }

            return new ServicesResult
            {
                isSuccess = true,
            };
        } 
        #endregion

    }
}
