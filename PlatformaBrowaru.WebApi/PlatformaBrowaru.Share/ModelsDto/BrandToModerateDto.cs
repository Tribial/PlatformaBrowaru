using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class BrandToModerateDto : BaseModelDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string UserNickname { get; set; }
        public decimal? Rate { get; set; }
        public decimal Alcohol { get; set; }
    }
}
