using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.Models
{
    public class Season
    {
        public string Name { get; set; }
        public virtual List<BrandSeason> BrandSeasons { get; set; }
    }
}
