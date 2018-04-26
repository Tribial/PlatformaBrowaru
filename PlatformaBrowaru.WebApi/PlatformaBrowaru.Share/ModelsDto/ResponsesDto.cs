using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class ResponsesDto<T> where T : BaseModelDto
    {
        public List<T> Object { get; set; }
        public bool ErrorOccured => Errors.Any();
        public List<string> Errors { get; set; }

        public ResponsesDto()
        {
            Errors = new List<string>();
        }
    }
}
