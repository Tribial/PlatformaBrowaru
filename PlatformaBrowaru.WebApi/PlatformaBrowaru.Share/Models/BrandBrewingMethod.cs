using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandBrewingMethod
    {
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public string BrewingMethod { get; set; }
    }
}
