using System;
using System.Collections.Generic;
using System.Text;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class SimpleBrewingGroupDto : BaseModelDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
