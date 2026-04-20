using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandController : Controller
    {
        private readonly CommandService _commandService;

        public CommandController(CommandService controlService)
        {
            _commandService = controlService;
        }

        // 📥 ESP32 gọi để lấy lệnh
        // GET: api/control/pending/1
        [HttpGet("pending/{lockerId}")]
        public async Task<IActionResult> GetPending(int lockerId)
        {
            var cmd = await _commandService.GetPendingCommand(lockerId);

            if (cmd == null)
                return NoContent(); // không có lệnh

            return Ok(cmd);
        }

        // ✅ ESP32 báo đã làm xong
        // POST: api/control/done/5
        [HttpPost("done/{id}")]
        public async Task<IActionResult> MarkDone(int id)
        {
            await _commandService.MarkDone(id);
            return Ok("Done");
        }
    }
}