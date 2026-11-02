using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparkySparky.Services;
//using TheThirdEye;

namespace SparkySparky.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Route("thired-eye")]
    [Route("third-eye")]
    public class TheThirdEyeController : ControllerBase
    {
        private readonly INeedleService needleService;

        public TheThirdEyeController(INeedleService needleService)
        {
            this.needleService = needleService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            needleService.InitializeNeedle();
            return Ok(new {
                message = "The Third Eye is watching you!",
                timestamp = DateTime.UtcNow,

            });
        }
    }
}
