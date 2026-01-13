using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Dtos.Responses;

namespace ServiceDefaults.Controllers
{
    public partial class ApiControllerBase : ControllerBase
    {
        public IActionResult CreateResult<T>(ApiResponse<T> response) => new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}
