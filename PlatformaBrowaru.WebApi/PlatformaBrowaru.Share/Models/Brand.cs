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
        public decimal AlcoholAmountPercent { get; set; }
        public decimal ExtractPercent { get; set; }
        public int HopIntensity { get; set; }
        public int TasteFullness { get; set; }//tutaj nie mam pojęcia jak inaczej to może wyglądać... xD
        public int Sweetness { get; set; }
        public virtual Kind Kind { get; set; }
        public bool IsPasteurized { get; set; }
        public bool IsFiltered { get; set; }
        public virtual List<BrandSeason> BrandSeasons { get; set; }
        public virtual List<BrandFermentationType> BrandFermentationTypes { get; set; }
        public virtual List<BrandBrewingMethod> BrandBrewingMethods { get; set; }
        public virtual List<BrandWrapping> BrandWrappings { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ApplicationUser AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
        public virtual ApplicationUser EditedBy { get; set; }
        public DateTime? EditedAt { get; set; }
        public virtual ApplicationUser DeletedBy { get; set; }
        public string DeletionReason { get; set; }
        public bool IsAccepted { get; set; }
    }
}
