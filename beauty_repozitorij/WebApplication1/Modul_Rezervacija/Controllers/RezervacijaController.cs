using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.EntityModels;
using WebApplication1.Helper.AutentifikacijaAutorizacija;
using WebApplication1.Modul_Rezervacija.ViewModels;

namespace WebApplication1.Modul_Rezervacija.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RezervacijaController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public RezervacijaController(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        [HttpPost]
        public ActionResult Add([FromBody] RezervacijaDodajVM x)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("Nije logiran!");

            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;

            if (korisnik == null)
                return BadRequest("Nemate ovlasti za trazenu akciju!");

            Rezervacija rezervacija = new Rezervacija()
            {
                KorisnikId = korisnik.Id,
                UslugaId = x.UslugaId,
                VrijemeRezervacije = x.DatumRezervacije
            };

            _dbContext.Rezervacija.Add(rezervacija);
            _dbContext.SaveChanges();

            return Ok(rezervacija);
        }
        [HttpGet]
        public List<RezervacijaGetAllVM> GetAll()
        {
            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;

            List<RezervacijaGetAllVM> rezervacije = _dbContext.Rezervacija.Where(x => x.KorisnikId == korisnik.Id)
                .Select(r => new RezervacijaGetAllVM()
                {
                    id = r.Id,
                    datumRezervacije = r.VrijemeRezervacije,
                    uslugaId = r.UslugaId,
                    nazivUsluge = r.Usluga.Naziv
                }).ToList();

            return rezervacije;
        }
        [HttpPost("{id}")]
        public ActionResult Delete(int id)
        {
            Rezervacija rezervacija = _dbContext.Rezervacija.Find(id);

            if (rezervacija == null)
                return BadRequest("pogresan ID");

            _dbContext.Remove(rezervacija);

            _dbContext.SaveChanges();
            return Ok(rezervacija);
        }
    }
}
