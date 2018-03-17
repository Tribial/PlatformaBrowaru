using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class Kind //Artur Karpinski
    {
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
        public virtual ApplicationUser AddedBy { get; set; } 
        public DateTime AddedAt { get; set; }
        public virtual ApplicationUser EditedBy { get; set; }
        public DateTime? EditedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual List<Brand> Brands { get; set; }
    }
}
