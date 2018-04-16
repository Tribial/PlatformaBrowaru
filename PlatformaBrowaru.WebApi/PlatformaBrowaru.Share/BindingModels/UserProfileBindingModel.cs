using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class UserProfileBindingModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Login powinien zawierać od 3 do 16 znaków", MinimumLength = 3)]
        public string Username { get; set; }
    }
}
