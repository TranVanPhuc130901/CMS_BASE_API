using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Database
{
    public class CartItems
    {
        public int CartItemID { get; set; }

        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
