using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.BindingModels
{
    public class ReviewSearchBindingModel
    {
        public int PageNumber { get; set; } = 1;
        public int Limit { get; set; } = 25;
        public string Sort { get; set; } = "Title";
        public string Query { get; set; } = "";
        public bool Ascending { get; set; } = true;
    }
}
