using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandProduction //Damian Jacyna
    {

        public DateTime ProducedFrom { get; set; }
        public DateTime ProducedTo { get; set; }
        public virtual Brewery ProducedBy { get; set; }
        public virtual Brand Brand { get; set; }
        public DateTime EditedAt { get; set; }
        public virtual ApplicationUser EditedBy { get; set; }

        public virtual Brewery Brewery { get; set; }
        public virtual List<Brand> Brands { get; set; }
    }
}
