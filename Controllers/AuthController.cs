using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using Microsoft.Extensions.Logging;
using ToDoList.Models.DTOs;

namespace ToDoList.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<IdentityUser> userManager, ILogger<AuthController> logger, ITokenService tokenService)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] CreateOrUpdateAuthDto authDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = authDto.username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, authDto.password!);

            if (identityResult.Succeeded)
            {
                if(authDto.Roles != null && authDto.Roles.Any())
                {
                    foreach (var role in authDto.Roles)
                    {
                        identityResult = await _userManager.AddToRoleAsync(identityUser, role);
                        if (!identityResult.Succeeded)
                        {
                            break;
                        }
                    }

                    if (identityResult.Succeeded)
                    {
                        return Ok("User registered successfully");
                    }
                }
            }
           
            return BadRequest(identityResult.Errors);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginAuthDto loginDto)
        {
             var user = await _userManager.FindByNameAsync(loginDto.username!);

             if (user != null)
             {
                var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.password!);
                
                if (passwordValid)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if(roles != null && roles.Any())
                    {
                        var token = _tokenService.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = token
                        };
                        
                        return Ok(response);
                    }
                }
             }

             return Unauthorized("Invalid username or password");
             
        }
        
    }
}