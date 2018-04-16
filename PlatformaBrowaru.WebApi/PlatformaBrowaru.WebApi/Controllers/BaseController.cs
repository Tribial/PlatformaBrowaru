using Microsoft.AspNetCore.Mvc;
using PlatformaBrowaru.Share.ModelsDto;

namespace PlatformaBrowaru.WebApi.Controllers
{
    public class BaseController : Controller
    {
        protected ResponseDto<BaseModelDto> ModelStateErrors()
        {
            var response = new ResponseDto<BaseModelDto>();

            foreach (var key in ModelState.Keys)
            {
                var value = ViewData.ModelState[key];

                foreach (var error in value.Errors)
                {
                    response.Errors.Add(error.Exception != null ? "Nieprawidłowy format danych" : error.ErrorMessage);
                }
            }
            return response;
        }
    }
}
