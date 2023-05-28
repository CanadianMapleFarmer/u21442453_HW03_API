using Assignment3_Backend.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using u21442453_HW03_API.Models;

namespace u21442453_HW03_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IRepository _repository;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IRepository repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _repository = repository;
        }

        //Registration
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            AppUser appUser = new AppUser()
            {
                UserName = model.emailaddress,
                Email = model.emailaddress,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.emailaddress);
                if (userExists != null) return StatusCode(StatusCodes.Status403Forbidden, "Account already exists");
                var result = await _userManager.CreateAsync(appUser, model.password);
                if (!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, "Registration failed, please try again.");

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Internal server error, please contact support...");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.emailaddress);

                if (user == null) return StatusCode(StatusCodes.Status404NotFound, "User not found...");

                if (!await _userManager.CheckPasswordAsync(user, model.password)) return StatusCode(StatusCodes.Status403Forbidden, "Password is incorrect...");

                var tokenObject = await GenerateJWTToken(user);
                string JwtToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);
                //await _userManager.SetAuthenticationTokenAsync(user, "JWT.io", "JWT Bearer Token", JwtToken);
                return JwtResponse(tokenObject, JwtToken, user);
            }
            catch (Exception)
            {
                return BadRequest("Internal server error, please contact support...");
            }
        }

        [HttpGet]
        private async Task<JwtSecurityToken> GenerateJWTToken(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                authClaims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(3)
            );

            return token;
        }

        [HttpGet]
        private ActionResult JwtResponse(JwtSecurityToken token, string JwtToken, AppUser user)
        {
            return Created("", new
            {
                token = JwtToken,
                expiry = token.ValidTo,
                user = user.UserName
            });
        }
        }
}
