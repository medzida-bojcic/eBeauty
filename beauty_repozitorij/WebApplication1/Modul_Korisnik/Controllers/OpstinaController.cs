using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.EntityModels;

namespace WebApplication1.Modul_Korisnik.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OpstinaController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public OpstinaController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public List<Opstina> GetAll()
        {
            return _dbContext.Opstina.ToList();
        }

        [HttpPost]
        public ActionResult Add(string naziv)
        {
            Opstina novaOpstina = new Opstina() { Naziv = naziv };
            _dbContext.Opstina.Add(novaOpstina);
            _dbContext.SaveChanges();
            return Ok(novaOpstina.Id);
        }
        [HttpPost("{id}")]
        public ActionResult Delete(int id)
        {
            Opstina opstina = _dbContext.Opstina.Find(id);

            if (opstina == null)//|| id == 1
                return BadRequest("pogresan ID");

            _dbContext.Remove(opstina);

            _dbContext.SaveChanges();
            return Ok(opstina);
        }
    }
}
