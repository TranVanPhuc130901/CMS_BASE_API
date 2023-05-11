using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_WT_API.Enums;

namespace CMS_Common.Model
{
    public class ProductAddDetailModel
    {
        public int ProductId { get; set; }

        public string? ProductCode { get; set; }

        public string? ProductName { get; set; }

        public string? ProductDescription { get; set; }
        public string? ProductContentName { get; set; }

        public ProductStatus ProductStatus { get; set; }

        public string? ProductImageSlug { get; set; }

        public string? ProductMetaDataTitle { get; set; }

        public string? ProductMetadataDescrition { get; set; }

        public decimal ProductCost { get; set; }

        public decimal ProductPromotional { get; set; }

        public int CategoryId { get; set; }
    }
}
