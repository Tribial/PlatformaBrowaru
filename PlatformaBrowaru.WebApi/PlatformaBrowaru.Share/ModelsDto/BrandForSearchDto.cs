using System;
using System.Collections.Generic;
using System.Text;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class BrandForSearchDto : BaseModelDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        //public SimpleBrewingGroupDto BrewingGroup { get; set; }
        public string KindName { get; set; }
        public Rating UserRate { get; set; }
        public decimal? Rate { get; set; }
        public decimal Alcohol { get; set; }
    }
}
