using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Common.Model
{
    public class ArticleModel
    {
        public int ArticleID { get; set; }
        /// <summary>
        /// Tiêu đề bài viết
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Nội dung bài viết
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// Chi tiết bài viết
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string? Image { get; set; }
        /// <summary>
        /// Tác giả
        /// </summary>
        public string? Author { get; set; }
        /// <summary>
        /// Thời gian tạo bài viết
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;
        /// <summary>
        /// Thời gian sửa bài viết
        /// </summary>
        public DateTime UpdateAt { get; set; }
        /// <summary>
        /// Bình luận của người viết
        /// </summary>
        public string ?ContentComment { get; set; }
        /// <summary>
        /// Từ khóa bài viết
        /// </summary>
        public string ?TagName { get; set; }
        /// <summary>
        /// Danh mục bài viết
        /// </summary>
        public string ?CategoryName { get; set; }
        /// <summary>
        /// Tên người bình luận
        /// </summary>
        public string ?UserName { get; set; }
    }
}
