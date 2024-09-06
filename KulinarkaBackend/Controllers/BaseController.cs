using Kulinarka.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResponse<T>(Response<T> response) {
            if (!response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            return Ok(response.Data);
        }
    }
}
