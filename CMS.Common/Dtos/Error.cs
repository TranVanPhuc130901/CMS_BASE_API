using CMS_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Dtos
{

        /// <summary>
        /// Đối tượng trả về khi gặp lội
        /// </summary>
        public class Error
        {
            /// <summary>
            /// Mã lỗi
            /// </summary>
            public ErrorCode? ErrorCode { get; set; }

            /// <summary>
            /// Msg lỗi cho Dev
            /// </summary>
            public string? DevMsg { get; set; }

            /// <summary>
            /// Msg lỗi cho user
            /// </summary>
            public string? UserMsg { get; set; }

            /// <summary>
            /// Thông tin thêm
            /// </summary>
            public object? MoreInfo { get; set; }

            /// <summary>
            /// Id lỗi
            /// </summary>
            public string? TradeID { get; set; }
        }
    
}
