using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandSeason
    {
        public long BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Season Season { get; set; }
        public long SeasonId { get; set; }
    }
}
