﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Database
{
    public class Cart
    {
        public int CartID { get; set; }

        public int UserID { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
