using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandWrapping
    {
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }

        public string WrappingName { get; set; }
    }
}
