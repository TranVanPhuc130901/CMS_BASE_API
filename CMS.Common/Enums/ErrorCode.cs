using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Enums
{
    public enum ErrorCode
    {
        /// <summary>
        /// Khi gặp Exception
        /// </summary>
        Exception = 0,

        /// <summary>
        /// Mã nhân viên trống
        /// </summary>
        EmployeeCodeIsEmpty = 1,

        /// <summary>
        /// Tên nhân viên trống
        /// </summary>
        EmployeeNameEmpty = 2,

        /// <summary>
        /// Phòng ban trống
        /// </summary>
        DepartMentIdIsEmpty = 3,

        /// <summary>
        /// Phòng ban không hợp lệ
        /// </summary>
        DepartMentIdIsNotValid = 4,

        /// <summary>
        /// Chức danh không hợp lệ
        /// </summary>
        PositionIdIsNotValid = 5,

        /// <summary>
        /// id không hợp lệ
        /// </summary>
        EmployeeIdIsNotValid = 6,

        /// <summary>
        /// Mã nhân viên đã tồn tại trong hệ thống
        /// </summary>
        EmployeeCodeIsDuplicate = 7,

        /// <summary>
        /// Dữ liệu đầu vào bị trống
        /// </summary>
        RequiredValueIsEmpty = 9,

        /// <summary>
        /// Mã nhân viên vượt quá độ dài cho phép
        /// </summary>
        OutLengthEmployeeCode = 10,

        /// <summary>
        /// Tên nhân viên vượt quá độ dài cho phép
        /// </summary>
        OutLengthEmployeeName = 11,

        /// <summary>
        /// Số chứng minh nhân dân vượt quá độ dài cho phép
        /// </summary>
        OutLengthIdentityNumber = 12,

        /// <summary>
        /// Nơi cấp vượt quá độ dài cho phép
        /// </summary>
        OutLengthIdentityIssuePlace = 13,

        /// <summary>
        /// Địa chỉ vượt quá độ dài cho phép
        /// </summary>
        OutLengthAddress = 14,

        /// <summary>
        /// Số điện thoại di động vượt quá độ dài cho phép
        /// </summary>
        OutLengthTelephoneNumber = 15,

        /// <summary>
        /// Số điện thoại cố định vượt quá độ dài cho phép
        /// </summary>
        OutLengthPhoneNumber = 16,

        /// <summary>
        /// Email vượt quá độ dài cho phép
        /// </summary>
        OutLengthEmail = 17,

        /// <summary>
        /// Tài khoản ngân hàng vượt quá độ dài cho phép
        /// </summary>
        OutLengthBankAccount = 18,

        /// <summary>
        /// Tên ngân hàng vượt quá độ dài cho phép
        /// </summary>
        OutLengthBankName = 19,

        /// <summary>
        /// Chi nhánh ngân hàng vượt quá độ dài cho phép
        /// </summary>
        OutLengthBankBranchName = 20,

        /// <summary>
        /// Email không đúng định dạng
        /// </summary>
        EmailInvalid = 21,

        /// <summary>
        /// Ngày sinh lớn hơn ngày hiện tại
        /// </summary>
        DateOfBirthInvalid = 22,

        /// <summary>
        /// Ngày cấp lớn hơn ngày hiện tại
        /// </summary>
        IdentityIssueDateInvalid = 23,

        /// <summary>
        /// Ngày cấp lớn hơn ngày hiện tại
        /// </summary>
        UpdateNotExist = 24,

        /// <summary>
        /// Ngày cấp lớn hơn ngày hiện tại
        /// </summary>
        DeleteNotExist = 25
    }
}
