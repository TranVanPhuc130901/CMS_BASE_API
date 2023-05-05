using CMS_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Database
{
    /// <summary>
    /// Bảng liên hệ, form thông tin người dùng
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// ID liên hệ
        /// </summary>
        public int ContactID { get; set; }
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string ?FullName { get; set; }
        /// <summary>
        ///  email khách hàng
        /// </summary>
        public string ?Email { get; set; }
        /// <summary>
        /// số điện thoại khách hàng
        /// </summary>
        public int PhoneNumber { get; set; }
        /// <summary>
        /// lời nhắn khách hàng
        /// </summary>
        public string ?Message { get; set; }
        /// <summary>
        /// Ngày khách hàng gửi yêu cầu
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
        /// <summary>
        /// TRạng thái
        /// </summary>
        public OrderStatus Status { get; set; }
    }
}
