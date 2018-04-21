using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class BrandProductionBindingModel
    {
        [Required]
        public DateTime ProducedFrom { get; set; }
        [Required]
        public DateTime ProducedTo { get; set; }
        [Required]
        public long BreweryId { get; set; }
    }
}
