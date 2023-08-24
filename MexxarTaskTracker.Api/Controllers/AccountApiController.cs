using AutoMapper;
using MexxarTaskTracker.Domain;
using MexxarTaskTracker.Domain.DTO;
using MexxarTaskTracker.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MexxarTaskTracker.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public AccountApiController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<object> Register([FromBody] UserDto userDetail)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(userDetail.Email).ConfigureAwait(false);
                if (userExist != null)
                {
                    _response.DisplayMessage = " User Already Exists";
                    _response.IsSuccess = false;
                }
                else
                {
                    var applicationUser = new ApplicationUser
                    {
                        Email = userDetail.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = userDetail.UserName,
                        Gender = userDetail.Gender,
                        SysCreatedOn = DateTime.Now
                    };

                    var result = await _userManager.CreateAsync(applicationUser, userDetail.Password).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync("Admin").ConfigureAwait(false))
                            await _roleManager.CreateAsync(new IdentityRole("Admin")).ConfigureAwait(false);

                        switch (userDetail.RoleType)
                        {
                            case RoleType.Admin:
                                if (await _roleManager.RoleExistsAsync("Admin").ConfigureAwait(false))
                                    await _userManager.AddToRolesAsync(applicationUser, new List<string> { "Admin" }).ConfigureAwait(false);

                                _response.Result = userDetail;

                                _response.DisplayMessage = "Successfully created new admin";
                                break;

                            case RoleType.User:
                                if (await _roleManager.RoleExistsAsync("User").ConfigureAwait(false))
                                    await _userManager.AddToRolesAsync(applicationUser, new List<string> { "User" }).ConfigureAwait(false);

                                _response.Result = userDetail;

                                _response.DisplayMessage = "Successfully created new user";
                                break;


                                _response.Result = userDetail;

                                _response.DisplayMessage = "Successfully created new vertical head";
                                break;
                        }
                    }
                    else
                    {
                        var errors = "";
                        foreach (var error in result.Errors)
                        {
                            errors += $"{error.Description} ";
                        }
                        _response.DisplayMessage = errors;
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<object> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email).ConfigureAwait(false);
                if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password).ConfigureAwait(false))
                {
                    var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                    var authSignKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                    var tokenHandler = new JwtSecurityTokenHandler();

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Id),
                            new Claim(ClaimTypes.Role,userRoles[0]),
                            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        }),
                        Issuer = _configuration["JWT:ValidIssuer"],
                        Audience = _configuration["JWT:ValidAudience"],
                        Expires = DateTime.Now.AddHours(1),
                        SigningCredentials = new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    //var validTo = token.ValidTo.ToString("yyyy-MM-ddThh:mm:ss");//
                    var bearerToken = tokenHandler.WriteToken(token);


                    var userDetails = new UserDto()
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Gender = user.Gender,
                        BearerToken = bearerToken,
                        SysCreatedOn = user.SysCreatedOn,
                    };

                    _response.Result = userDetails;

                    _response.DisplayMessage = userDetails == null ? "Empty user details" : "Get user details";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Unauthorized user - Incorrect user credentials";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet]
        [Route("ValidateJwtToken/{token}")]
        public string ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string accountId = jwtToken.Claims.First(x => x.Type == "unique_name").Value;

                // return account id from JWT token if validation successful
                return accountId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        [HttpGet]
        [Route("ValidateMobileToken/{token}")]
        public string ValidateMobileToken(string token)
        {
            try
            {
                var stream = token;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;

                var userid = tokenS.Claims.First(claim => claim.Type == "sub").Value;
                return userid;
            }
            catch
            {
                return null;
            }

        }

        [HttpGet]
        [Route("GetUserRoles/{token}")]
        public async Task<object> GetUserRoles(string token)
        {
            try
            {
                var userId = ValidateJwtToken(token);
                if (userId != null)
                {
                    var response = (ResponseDto)GetUserDetails(userId);
                    ApplicationUser userDetails = null;

                    if (response?.Result != null)
                    {
                        var User = (UserDto)response.Result;
                        if (User != null)
                        {
                            userDetails = new ApplicationUser
                            {
                                Id = User.UserId,
                                Email = User.Email,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                UserName = User.UserName,
                                Gender = User.Gender,
                                SysCreatedOn = DateTime.Now
                            };
                        }
                    }

                    if (userDetails != null)
                    {
                        var roles = await _userManager.GetRolesAsync(userDetails).ConfigureAwait(false);
                        if (roles != null)
                        {
                            _response.Result = roles;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet]
        [Route("GetAllUsers/{Role}")]
        public async Task<object> GetAllUsers(string Role)
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync(Role).ConfigureAwait(false);
                if (users != null)
                {
                    var result = _mapper.Map<List<UserDto>>(users.ToList());
                    _response.Result = result;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet]
        [Route("GetUsersByMail/{mail}")]
        public object GetUsersByMail(string mail)
        {
            try
            {
                var users = _userManager.Users.Where(u => u.Email.Contains(mail)).ToList();
                if (users?.Count > 0)
                {
                    var result = _mapper.Map<List<UserDto>>(users.ToList());
                    _response.Result = result;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet]
        [Route("GetUserDetails/{userId}")]
        public object GetUserDetails(string userId)
        {
            try
            {
                var userDetails = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (userDetails != null)
                {
                    _response.Result = new UserDto()
                    {
                        UserId = userDetails.Id,
                        UserName = userDetails.UserName,
                        Email = userDetails.Email,
                        Gender = userDetails.Gender,
                        SysCreatedOn = userDetails.SysCreatedOn,
                    };
                }
                else
                {
                    _response.Result = new UserDto();
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "User Not Found";
                    _response.ErrorMessages = new List<string>() { "User Not Found" };
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

    }
}
