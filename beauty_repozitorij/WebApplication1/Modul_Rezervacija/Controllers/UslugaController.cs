using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.EntityModels;

namespace WebApplication1.Modul_Rezervacija.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UslugaController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public UslugaController(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        [HttpGet]
        public List<Usluga> GetAll()
        {
            return _dbContext.Usluga.ToList();
        }
    }
}
