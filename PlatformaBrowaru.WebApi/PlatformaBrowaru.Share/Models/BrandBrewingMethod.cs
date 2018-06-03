using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandBrewingMethod
    {
        public long BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public long BrewingMethodId { get; set; }
        public virtual BrewingMethod BrewingMethod { get; set; }
    }
}
