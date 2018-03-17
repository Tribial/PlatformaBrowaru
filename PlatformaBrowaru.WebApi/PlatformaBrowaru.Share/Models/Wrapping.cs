using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class Wrapping
    {
        public string Name { get; set; }
        public virtual List<BrandWrapping> BrandWrappings { get; set; }
    }
}
