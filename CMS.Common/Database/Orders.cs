using CMS_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Database
{
    public class Orders
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }
        public DateTime  OrderDate { get; set; } = DateTime.Now;

        public OrderStatus OrderStatus { get; set; }

        public Decimal TotalPrice { get; set; }
    }
}
