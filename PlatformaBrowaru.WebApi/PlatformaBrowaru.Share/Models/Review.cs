using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class Review // Artur Karpinski
    {
        public virtual ApplicationUser Author { get; set; }
        public virtual Brand Brand { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public bool IsDleted { get; set; }
    }
}
