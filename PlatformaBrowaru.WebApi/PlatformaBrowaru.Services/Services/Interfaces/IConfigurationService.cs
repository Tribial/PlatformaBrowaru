using System.Collections.Generic;
using System.Text;

namespace PlatformaBrowaru.Services.Services.Interfaces
{
    public interface IConfigurationService
    {
        string GetValue(string key);
    }
}
