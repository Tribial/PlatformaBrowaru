using System;
using System.Collections.Generic;
using System.Text;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IBreweryRepository
    {
        Brewery Get(Func<Brewery, bool> function);
    }
}
