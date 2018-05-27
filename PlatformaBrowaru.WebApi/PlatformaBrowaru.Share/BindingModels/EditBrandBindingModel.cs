using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class EditBrandBindingModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Nazwa powinna zawierać od 3 do 30 znaków", MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Opis powinien zawierać do 10 do 255 znaków", MinimumLength = 10)]
        public string Description { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Składniki powinny zawierać od 10 do 255 znaków", MinimumLength = 10)]
        public string Ingredients { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Barwa powinna zawierać od 3 do 30 znaków", MinimumLength = 3)]
        public string Color { get; set; }
        [Required]
        [Range(0, 30, ErrorMessage = "Zawartość alkoholu powinna być w przedziale od 0% do 30%")]
        public decimal AlcoholAmountPercent { get; set; }
        [Required]
        [Range(0, 30, ErrorMessage = "Ekstrakt powinien być w przedziale od 0% do 30%")]
        public decimal ExtractPercent { get; set; }
        [Range(0, 5, ErrorMessage = "Chmielowość oznaczona w skali od 0 do 5")]
        public int? HopIntensity { get; set; }
        [Range(0, 5, ErrorMessage = "Pełnia smaku oznaczona w skali od 0 do 5")]
        public int? TasteFullness { get; set; }
        [Range(0, 5, ErrorMessage = "Słodycz oznaczona w skali od 0 do 5")]
        public int? Sweetness { get; set; }
        [Required]
        public long KindId { get; set; }
        [Required]
        public bool IsPasteurized { get; set; }
        [Required]
        public bool IsFiltered { get; set; }
        public List<long> SeasonIds { get; set; }
        public List<long> FermentationTypeIds { get; set; }
        public List<long> BrewingMethodIds { get; set; }
        public List<long> WrappingIds { get; set; }
        //public BrandProductionBindingModel BrandProduction { get; set; } = null;
        public DateTime? CreationDate { get; set; } = null;
    }
}
