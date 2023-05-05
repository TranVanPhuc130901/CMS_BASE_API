using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Enums
{
    public enum OrderStatus
        {
        /// <summary>
        /// Đax giao hàng
        /// </summary>
        Delivered = 0,
        /// <summary>
        /// Chờ xử lý
        /// </summary>
        Pending = 1,
        /// <summary>
        /// Chưa giao hàng
        /// </summary>
        Undelivered = 2
    }
}
