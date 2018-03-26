using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class Rating // Artur Karpinski
    {
        public long Id { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual Brand Brand { get; set; }
        public decimal Rate { get; set; }

    }
}
