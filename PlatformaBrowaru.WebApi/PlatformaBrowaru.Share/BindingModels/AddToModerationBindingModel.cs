using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class AddToModerationBindingModel
    {
        [Required] public long BrandId { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Powód powinien zawierać od 3 do 25 znaków", MinimumLength = 3)]
        public string Reason { get; set; }
    }
}
