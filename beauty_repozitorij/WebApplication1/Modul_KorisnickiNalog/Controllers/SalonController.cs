using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Modul_KorisnickiNalog.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SalonController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public SalonController(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }
        [HttpGet]
        public ActionResult GetByAll()
        {
            return Ok(_dbContext.Salon.ToList()); 
        }
    }
}
