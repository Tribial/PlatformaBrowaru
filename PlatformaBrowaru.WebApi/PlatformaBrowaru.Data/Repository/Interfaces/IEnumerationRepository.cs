using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Repository.Interfaces
{
    public interface IEnumerationRepository
    {
        Season GetSeasonWithoutTracking(Func<Season, bool> function);
        BrewingMethod GetBrewingMethodWithoutTracking(Func<BrewingMethod, bool> function);
        Wrapping GetWrappingWithoutTracking(Func<Wrapping, bool> function);
        FermentationType GetFermentationWithoutTracking(Func<FermentationType, bool> function);
        Season GetSeason(Func<Season, bool> function);
        BrewingMethod GetBrewingMethod(Func<BrewingMethod, bool> function);
        Wrapping GetWrapping(Func<Wrapping, bool> function);
        FermentationType GetFermentation(Func<FermentationType, bool> function);
        Task<bool> ClearEnumerationsForBrand(long brandId);
    }
}
