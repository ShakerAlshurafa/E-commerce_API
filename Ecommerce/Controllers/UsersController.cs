using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<LocalUser> userManager;
        private readonly IEmailService emailService;

        public UsersController(IUserRepository userRepository, 
                                UserManager<LocalUser> userManager,
                                IEmailService emailService)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.emailService = emailService;
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


        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmailForUser(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return BadRequest(new ApiValidationResponse(new List<string> { $"This Email {email} not found !" }));
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var forgetPasswordLink = Url.Action(
                "ResetPassword", 
                "Users", 
                new { token = token, email = email }, 
                Request.Scheme
            );
            var subject = "Reset Password Request";
            var message = $"Please click on the link to reset your password: {forgetPasswordLink}";

            await emailService.SendEmailAsync(email, subject, message);

            return Ok(new ApiRespons(200, "Password reset link has been sent to your email.. Check Your Email"));
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if(ModelState.IsValid)
            {
                if(string.Compare(model.confirmNewPassword,model.newPassword) != 0)
                {
                    return BadRequest(new ApiRespons(400, "notMatch"));
                }
                if (string.IsNullOrEmpty(model.Token))
                {
                    return BadRequest(new ApiRespons(400, "Token Invalid"));
                }

                var user = await userManager.FindByEmailAsync(model.Email);
                if(user == null)
                {
                    return BadRequest(new ApiRespons(404, "Email Incorrect"));
                }

                var result = await userManager.ResetPasswordAsync(user, model.Token, model.newPassword);
                if (result.Succeeded)
                {
                    return Ok(new ApiRespons(200, "Reseting Successfully"));
                }
                else
                {
                    return BadRequest(new ApiRespons(400, "Error while reseting"));
                }
            }
            return BadRequest(new ApiRespons(400, "Check your info"));
        }

        [HttpGet("reset-token")]
        public async Task<IActionResult> TokenToResetPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return NotFound(new ApiRespons(404));
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            return Ok(new {token = token});
        }
    }
}
