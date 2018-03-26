using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class LoginBindingModel
    {
        [Required]
        [EmailAddress]
        [StringLength(40, ErrorMessage = "Email powinien zawierać od 3 do 40 znaków", MinimumLength = 3)]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [StringLength(30, ErrorMessage = "Hasło powinno zawierać od 7 do 30 znaków", MinimumLength = 7)]
        public string Password { get; set; }
    }
}
