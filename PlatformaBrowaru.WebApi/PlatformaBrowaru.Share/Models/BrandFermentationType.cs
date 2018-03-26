using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandFermentationType
    {
        public long BrandId { get; set; }
        public Brand Brand { get; set; }
        public long FermentationTypeId { get; set; }
        public FermentationType FermentationType { get; set; }
    }
}
