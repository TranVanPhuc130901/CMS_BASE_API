using CMS_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.CustomAttribute
{
    public class MyMaxLengthAttribute : Attribute
    {
        /// <summary>
        /// Chiều dài tối đa
        /// </summary>
        public int MaxLength { get; set; }

        public ErrorCode? ErrorCode { get; set; }

        public MyMaxLengthAttribute(int maxLength, ErrorCode errorCode)
        {
            MaxLength = maxLength;

            ErrorCode = errorCode;
        }
    }
}
