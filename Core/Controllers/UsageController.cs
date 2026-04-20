using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsageController : Controller
    {
        private readonly UsageService _usageService;

        public UsageController(UsageService usageService)
        {
            _usageService = usageService;
        }

        // POST: api/usage/start
        [HttpPost("start")]
        public async Task<IActionResult> StartUsage(int userId, int lockerId)
        {
            var code = await _usageService.StartUsage(userId, lockerId);

            if (code != null)
            {
                return Ok(new
                {
                    message = "Started",
                    accessCode = code
                });
            }

            return BadRequest("Cannot start usage");
        }

        // POST: api/usage/end
        [HttpPost("end")]
        public async Task<IActionResult> EndUsage(int lockerId)
        {
            var result = await _usageService.EndUsage(lockerId);

            if (result)
                return Ok("Ended");

            return BadRequest("Cannot end usage");
        }

        // 📄 Get usage by id
        // GET: api/usage/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsageById(int id)
        {
            var usage = await _usageService.GetUsageById(id);

            if (usage == null)
                return NotFound("Usage not found");

            return Ok(usage);
        }

        // 📚 Get all usage
        // GET: api/usage
        [HttpGet]
        public async Task<IActionResult> GetAllUsage()
        {
            var list = await _usageService.GetAllUsage();
            return Ok(list);
        }

        // 🔄 Get active usage by locker
        // GET: api/usage/active/5
        [HttpGet("active/{lockerId}")]
        public async Task<IActionResult> GetActiveUsage(int lockerId)
        {
            var usage = await _usageService.GetActiveUsage(lockerId);

            if (usage == null)
                return NotFound("No active usage");

            return Ok(usage);
        }

        // 👤 Get history by user
        // GET: api/usage/user/3
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetHistory(int userId)
        {
            var list = await _usageService.GetUsageHistoryByUser(userId);
            return Ok(list);
        }


        [HttpGet("calculate/{lockerId}")]
        public async Task<IActionResult> Calculate(int lockerId)
        {
            var result = await _usageService.Calculate(lockerId);

            if (result == null)
                return NotFound("No active usage");

            return Ok(result);
        }


        [HttpPost("open-by-code")]
        public async Task<IActionResult> OpenByCode(string code)
        {
            var result = await _usageService.OpenByCode(code);

            if (result)
                return Ok("Locker opened");

            return BadRequest("Invalid or expired code");
        }

        [HttpPost("end-by-code")]
        public async Task<IActionResult> EndByCode(string code)
        {
            var result = await _usageService.EndUsageByCode(code);

            if (result)
                return Ok("Ended successfully");

            return BadRequest("Invalid or expired code");
        }
        [HttpPost("calculate-by-code")]
        public async Task<IActionResult> CalculateByCode(string code)
        {
            var result = await _usageService.CalculateByCode(code);

            if (result == null)
                return BadRequest("Invalid or expired code");

            return Ok(result);
        }
    }
}