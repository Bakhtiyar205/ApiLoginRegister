using LoginRegisterJWT.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginRegisterJWT.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _appDbContext.Employee.ToListAsync());
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllByName")]
        public async Task<IActionResult> GetAllByName([FromBody] string name)
        {
            return Ok(await _appDbContext.Employee.Where(m=>m.FullName == name).ToListAsync());
        }
    }
}
