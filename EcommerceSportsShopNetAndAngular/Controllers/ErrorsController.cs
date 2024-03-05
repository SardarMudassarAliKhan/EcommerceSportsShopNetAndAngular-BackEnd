using EcommerceSportsShopNetAndAngular.Errors;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSportsShopNetAndAngular.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    public class ErrorsController : BaseApiController
    {
        [HttpGet]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
