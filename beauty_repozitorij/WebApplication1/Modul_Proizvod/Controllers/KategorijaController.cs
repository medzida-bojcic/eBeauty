using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.EntityModels;

namespace WebApplication1.Modul_Proizvod.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class KategorijaController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public KategorijaController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public List<Kategorija> GetAll()
        {
            return _dbContext.Kategorija.ToList();
        }
    }
}
