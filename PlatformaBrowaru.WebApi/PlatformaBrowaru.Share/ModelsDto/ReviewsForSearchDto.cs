using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class ReviewsForSearchDto : BaseModelDto
    {
        public long Id { get; set; }
        public string UserNickname { get; set; }
        public string Title { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
