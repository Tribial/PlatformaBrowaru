using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class FermentationType
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public virtual List<BrandFermentationType> BrandFermentationTypes { get; set; }
    }
}
