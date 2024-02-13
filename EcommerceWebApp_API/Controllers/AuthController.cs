using EcommerceWebApp_API.Data;
using EcommerceWebApp_API.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EcommerceWebApp_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Tilleggslogikk etter vellykket registrering, som automatisk innlogging eller sending av en bekreftelses-epost
                return Ok(new { UserId = user.Id });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Suksessfull innlogging, returner passende respons
                return Ok();
            }
            else
            {
                // Innlogging mislyktes, returner feil
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            // Suksessfull utlogging, returner passende respons
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update the user properties
            user.Email = model.Email;
            user.UserName = model.Email;
            // Add other properties you want to update

            // Save the changes
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // If the update fails, add the errors to the model state and return a bad request response
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // Eventuelle andre metoder 
    }
}
