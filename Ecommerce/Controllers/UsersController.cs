using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await userRepository.Login(model);
                if(user.User == null)
                {
                    return Unauthorized(new ApiValidationResponse(new List<string>() { "Email or password inCorrect"}, 401));
                }
                return Ok(user);
            }
            return BadRequest(new ApiValidationResponse(new List<string>() { "Please try to enter the email and password correctly" }, 401));
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterationRequestDTO model)
        {
            try
            {
                bool uniqueEmail = userRepository.IsUniqueUser(model.Email);
                if (!uniqueEmail)
                {
                    return BadRequest(new ApiRespons(400, "Email already exist !"));
                }
                var user = userRepository.Register(model);
                if (user.Result == null)
                {
                    return BadRequest(new ApiRespons(400, "Error while registeration User!"));
                }
                return Ok(new ApiRespons(201, result: user.Result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiValidationResponse(new List<string> { ex.Message, "An error occurred whlie processing your request!" }));
            }
        }
    }
}
