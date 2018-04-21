using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandWrapping
    {
        public long BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Wrapping Wrapping { get; set; }
        public long WrappingId { get; set; }
    }
}
