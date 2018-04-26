using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class DeleteBeerBrandBindingModel
    {
        [Required]
        public string DeletionReason { get; set; }
    }
}
