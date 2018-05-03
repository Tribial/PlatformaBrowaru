using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandToModerate
    {
        public long Id { get; set; }
        public Brand Brand { get; set; }
        public ApplicationUser User { get; set; }
        public string Reason { get; set; }
    }
}
