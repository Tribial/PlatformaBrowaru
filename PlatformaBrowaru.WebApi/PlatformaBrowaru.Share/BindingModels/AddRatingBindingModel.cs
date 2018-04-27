using System.ComponentModel.DataAnnotations;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class AddRatingBindingModel
    {
        [Required]
        public decimal Rating { get; set; }
    }
}