using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Share.ModelsDto
{
    public class GetUserListDto : BaseModelDto
    {
        public List<GetUserDto> ListUsers { get; set; }

        public GetUserListDto()
        {
            ListUsers = new List<GetUserDto>();
        }
    }
}
