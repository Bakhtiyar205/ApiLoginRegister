using LoginRegisterJWT.Data;
using LoginRegisterJWT.DTOs.Account;
using LoginRegisterJWT.Models;
using LoginRegisterJWT.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginRegisterJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenService tokenService;

        public AccountController(AppDbContext context,
                                UserManager<AppUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                ITokenService tokenService)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;
        }
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            AppUser user = new()
            {
                FullName = registerDTO.FullName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            await userManager.CreateAsync(user, registerDTO.Password);
            //await userManager.AddToRoleAsync(user, "admin");

            return Ok();
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromQuery] string role)
        {
            await this.roleManager.CreateAsync(new IdentityRole { Name = role });
            return Ok();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await this.userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) NotFound();

            if (!await this.userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized();

            var roles = await this.userManager.GetRolesAsync(user);

            string token = this.tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);

            return Ok(token);
        }
    }
}
