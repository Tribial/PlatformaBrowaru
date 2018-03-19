using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class BrandFermentationType
    {
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public string FermentationType { get; set; }
    }
}
