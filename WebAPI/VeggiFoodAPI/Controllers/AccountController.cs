using GAMBULL_GAMC.UTILITY.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VeggiFoodAPI.Data;
using VeggiFoodAPI.Helpers;
using VeggiFoodAPI.Models.DTOs;
using VeggiFoodAPI.Models.ViewModels;
using VeggiFoodAPI.Services;

namespace VeggiFoodAPI.Controllers
{

    [Route("api/account")]
    [LogAttribute]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        CustomResponse _customResponse = new CustomResponse();
        public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("allusers")]
        public ActionResult<List<UserDetails>> GetAllUsers()
        {
            var data = from role in _context.Roles
                   join userRole in _context.UserRoles on role.Id equals userRole.RoleId
                   join user in _context.Users on userRole.UserId equals user.Id
                   select new UserDetails
                   {
                       Email = user.Email,
                       Username = user.UserName,
                       Role = role.Name,
                       Address = user.Address,
                   };
            return Ok(_customResponse.GetResponseModel(null, data));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel register)
        {
            //throw new Exception("The student cannot be found.");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = register.Username, Email = register.Email };

                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "member");
                    return Ok(_customResponse.GetResponseModel(null, "User registered successfully"));
                }
                  
                return Conflict(_customResponse.GetResponseModel( result.Errors.Select(x => x.Description), null));
            }
            return BadRequest(_customResponse.GetResponseModel( ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)),null));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDetails>> Login([FromBody] LoginModel loginDto)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.FindByNameAsync(loginDto.Username);

                if (currentUser == null || !await _userManager.CheckPasswordAsync(currentUser, loginDto.Password))
                {
                    return Unauthorized(_customResponse.GetResponseModel( new string[] { "Invalid Credentials" }, null));
                }

                string token = await _tokenService.GenerateToken(currentUser);

                var userDetails = from role in _context.Roles
                                  join userRole in _context.UserRoles on role.Id equals userRole.RoleId
                                  where userRole.UserId == currentUser.Id
                                  select new UserDetails
                                  {
                                      Email = currentUser.Email,
                                      Username = currentUser.UserName,
                                      Role = role.Name,
                                      Token = token
                                  };

                return Ok(_customResponse.GetResponseModel(null, userDetails.FirstOrDefault()));
            }

            return BadRequest(_customResponse.GetResponseModel( ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)),null));
        }

    }
}
