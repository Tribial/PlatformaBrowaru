using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class RegisterBindingModel
    {

        [Required]
        [EmailAddress]
        [StringLength(40, ErrorMessage = "Email powinien zawierać od 3 do 40 znaków", MinimumLength = 3)]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [StringLength(30, ErrorMessage = "Hasło powinno zawierać od 7 do 30 znaków", MinimumLength = 7)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword{ get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Login powinien zawierać od 3 do 16 znaków", MinimumLength = 3)]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string Role { get; set; }

        public string Gender { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM.dd.yyyy}")]
        
        public DateTime DateOfBirth { get; set; }
    }
}
