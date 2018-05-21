using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class GetUserDto : BaseModelDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
