﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class KindDto : BaseModelDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Description { get; set; }
    }
}
