using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LockerController : Controller
    {
        private readonly LockerService _lockerService;
        public LockerController(LockerService lockerService)
        {
            _lockerService = lockerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLockers()
        {
            var lockers = await _lockerService.GetAllLockers();
            return Ok(lockers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLockerById(int id)
        {
            var locker = await _lockerService.GetLockerById(id);

            if (locker == null)
                return NotFound("Locker not found");

            return Ok(locker);
        }

        [HttpGet("{id}/available")]
        public async Task<IActionResult> IsAvailable(int id)
        {
            var exists = await _lockerService.GetLockerById(id);
            if (exists == null)
                return NotFound("Locker not found");

            var isAvailable = await _lockerService.IsAvailable(id);
            return Ok(new { available = isAvailable });
        }
    }
}
