using Microsoft.AspNetCore.Mvc;
using theUpSkilzAPI.Data; // ✅ match the correct casing

namespace theUpSkilzAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult TestConnection()
        {
            return Ok("Database Connected!");
        }
    }
}
