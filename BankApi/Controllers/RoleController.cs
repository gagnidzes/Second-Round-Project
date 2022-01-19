using BankProjectApi.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RoleController> logger
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllRoles")]

        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();

            return Ok(roles);
        }
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string name)
        {
            var roleExis = await _roleManager.RoleExistsAsync(name);

            if (!roleExis)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"Role {name} creted successfully");
                    return Ok(new
                    {
                        result = $"Role {name} creted successfully"
                    });
                }
                else
                {
                    _logger.LogInformation($"Role {name}cant creted ");
                    return BadRequest(new
                    {
                        result = $"Role {name}cant creted "
                    });
                }
            }

            return BadRequest(new { error = "Role Already Exist" });

        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        [Route("AddUserToRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]

        public async Task<IActionResult> AddUserToRole(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                _logger.LogInformation($"User {userName} not exist ");
                return BadRequest(new
                {
                    error = $"User {userName} not exist "
                });
            }
            var roleExis = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExis)
            {
                _logger.LogInformation($"Role {roleName} not exist ");
                return BadRequest(new
                {
                    error = $"Role {roleName} not exist "
                });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = "Success,user added role"
                });
            }
            else
            {
                _logger.LogInformation($"user cant add to this role ");
                return BadRequest(new
                {
                    error = $"user cant add to this role "
                });
            }
        }

        [HttpGet]
        [Route("GetUserRoles")]

        public async Task<IActionResult> GetUserRoles(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                _logger.LogInformation($"User {userName} not exist ");
                return BadRequest(new
                {
                    error = $"User {userName} not exist "
                });
            }
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }
        [HttpPost]
        [Route("RemoveUserRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]

        public async Task<IActionResult> RemoveUserRole(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                _logger.LogInformation($"User {userName} not exist ");
                return BadRequest(new
                {
                    error = $"User {userName} not exist "
                });
            }
            var roleExis = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExis)
            {
                _logger.LogInformation($"Role {roleName} not exist ");
                return BadRequest(new
                {
                    error = $"Role {roleName} not exist "
                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"{user} removed role {roleName}"
                });
            }
            else
            {
                return BadRequest(new
                {
                    error = $"Cant remove {user} from role {roleName}"
                });
            }

        }

    }
}
