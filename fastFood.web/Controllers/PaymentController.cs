
using Microsoft.AspNetCore.Mvc;

namespace fastFood.web.Controllers
{
    [Route("api/payment")]
    public class PaymentController : Controller
    {
        [HttpPost("token")]
        public IActionResult GenerateToken()
        {
            return Json(new { token = "dummy-snap-token" });
        }

        [HttpPost("notification")]
        public IActionResult Notification()
        {
            return Ok();
        }
    }
}
