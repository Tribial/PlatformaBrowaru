using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IEnumerationRepository
    {
        Season GetSeason(Func<Season, bool> function);
        BrewingMethod GetBrewingMethod(Func<BrewingMethod, bool> function);
        Wrapping GetWrapping(Func<Wrapping, bool> function);
        FermentationType GetFermentation(Func<FermentationType, bool> function);
        Task<bool> ClearEnumerationsForBrand(long brandId);
    }
}
