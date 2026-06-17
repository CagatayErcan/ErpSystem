using Microsoft.AspNetCore.Mvc;
using Erp.Data;
using Erp.Data.Entities;

namespace Erp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ErpDbContext _context;

        public TestController(ErpDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "ERP API is running!",
                databaseStatus = "Connection is ready",
                serverTime = DateTime.Now
            });
        }

        [HttpPost("add-company")]
        public async Task<IActionResult> AddCompany([FromBody] string name)
        {
            var company = new Company { Name = name };
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return Ok(new { id = company.Id, name = company.Name });
        }
    }
}