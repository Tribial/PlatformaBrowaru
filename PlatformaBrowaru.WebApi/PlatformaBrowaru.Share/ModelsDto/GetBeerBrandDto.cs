using System;
using System.Collections.Generic;
using System.Text;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class GetBeerBrandDto : BaseModelDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Color { get; set; }
        public decimal AlcoholAmountPercent { get; set; }
        public decimal ExtractPercent { get; set; }
        public int HopIntensity { get; set; }
        public int TasteFullness { get; set; }
        public int Sweetness { get; set; }
        public string Kind { get; set; }
        public bool IsPasteurized { get; set; }
        public bool IsFiltered { get; set; }
        public List<string> BrandSeasons { get; set; }
        public string BrandFermentationType { get; set; }
        public string BrandBrewingMethod { get; set; }
        public List<string> BrandWrappings { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? GeneralRate { get; set; }
        public decimal? YourRate { get; set; }
    }
}
