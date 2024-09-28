using aiProj.DAL;
using aiProj.Domian;
using Microsoft.AspNetCore.Mvc;

namespace aiProj.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private UserService _userService;
        
        public UserController(UserService userService)
        { 
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return Ok(UserService.Code);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            User user = new User()
            {
                Login = userViewModel.Username,
                Email = userViewModel.Email,
                Password = userViewModel.Password,
            };
            var code = await _userService.Register(user);

            if (!string.IsNullOrEmpty(code))
            {
                return Ok(code);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCode()
        {
            var confirmed = await _userService.ConfirmCode();

            if (confirmed)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVeiwModel loginVeiwModel)
        {
            var result = await _userService.Login(loginVeiwModel);

            return Ok(result);
        }
    }
}
