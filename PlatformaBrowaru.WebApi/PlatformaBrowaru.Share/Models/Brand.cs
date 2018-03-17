using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class Brand // Fabian Domurad
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Color { get; set; }
        public DateTime DateOfCreation { get; set; }
        public decimal AlcoholAmountPercent { get; set; }
        public decimal ExtractPercent { get; set; }
        public int HopIntensity { get; set; }
        
    }
}
