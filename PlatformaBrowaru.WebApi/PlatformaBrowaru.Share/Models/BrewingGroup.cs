using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrewingGroup //Damian Jacyna
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string Adress { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
        public string DeletionReason { get; set; }
        public DateTime EditedAt { get; set; }
        public virtual ApplicationUser EditedBy { get; set; }

        public virtual List<Brewery> Breweries { get; set; }


    }
}
