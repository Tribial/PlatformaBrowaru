using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class Brewery //Damian Jacyna
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Description { get; set; }
        public decimal AnnualProduction { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual BrewingGroup Owner { get; set; }
        public virtual ApplicationUser AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
        public string DeletionReason { get; set; }
        public DateTime EditedAt { get; set; }
        public virtual ApplicationUser EditedBy { get; set; }
        public virtual List<BrandProduction> BrandProductions { get; set; }
    }
}
