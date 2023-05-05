using CMS_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Dtos
{
    public class ServicesResult
    {
        public Boolean isSuccess;
        public ErrorCode? ErrorCode;
        public int? numberOfAffectedRows;
        public object? Error;
    }
}
