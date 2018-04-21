using System;
using System.Collections.Generic;
using System.Text;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IKindRepository
    {
        Kind Get(Func<Kind, bool> function);
    }
}
