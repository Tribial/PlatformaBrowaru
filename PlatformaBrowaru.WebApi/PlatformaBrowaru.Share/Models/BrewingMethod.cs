using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrewingMethod
    {
        public long Id { get; set; }
        public string Method { get; set; }
        public  virtual List<BrandBrewingMethod> BrandBrewingMethods { get; set; }
    }
}
