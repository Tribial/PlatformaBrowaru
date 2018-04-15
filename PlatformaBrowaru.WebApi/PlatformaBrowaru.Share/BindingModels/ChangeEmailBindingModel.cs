using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class ChangeEmailBindingModel
    {
        [Required]
        [EmailAddress]
        [StringLength(40, ErrorMessage = "Email powinien zawierać od 3 do 40 znaków", MinimumLength = 3)]
        public string NewEmail { get; set; }

        [Compare("NewEmail")]
        public string ConfirmNewEmail { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

    }
}
