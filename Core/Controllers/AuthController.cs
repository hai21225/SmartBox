using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO u)
        {
            if(u == null) { return BadRequest(); }
            var user=await _auth.Login(u);
            if ( user!=null)
            {
              
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpPost("register")] public async Task<IActionResult> Register([FromBody]UserDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            bool isSuccess = await _auth.Register(user, user.ConfirmPass);
            if ( isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
