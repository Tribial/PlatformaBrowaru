using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class ReviewDto : BaseModelDto
    {
        public string BrandName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? EditionDate { get; set; }
        public string UserNickname { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
