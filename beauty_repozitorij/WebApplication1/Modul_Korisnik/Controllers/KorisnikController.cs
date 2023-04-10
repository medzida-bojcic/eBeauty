using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.EntityModels;
using WebApplication1.Helper;
using WebApplication1.Helper.AutentifikacijaAutorizacija;
using WebApplication1.Modul_Korisnik.ViewModels;

namespace WebApplication1.Modul_Korisnik.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class KorisnikController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public KorisnikController(ApplicationDbContext _dbContext)
        {
            dbContext=_dbContext;
        }

        [HttpPost]

        public ActionResult Add([FromBody] RegistracijaVM registracija)
        {
            Korisnik noviKorisnik = new Korisnik()
            {
                Ime = registracija.ime,
                Prezime = registracija.prezime,
                KorisnickoIme = registracija.username,
                Lozinka = registracija.password,
                Email = registracija.email,
                Adresa = registracija.adresa,
                BrojTelefona = registracija.brojTelefona,
                OpstinaId = registracija.opstinaId
            };

            dbContext.Korisnik.Add(noviKorisnik);
            dbContext.SaveChanges();

            return Ok(noviKorisnik);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;
            List<Narudzba> trenutneNarudzbe = dbContext.Narudzba.Where(n => n.KorisnikId == korisnik.Id && n.StatusNarudzbeId != 1 && n.Zakljucena).ToList();
            if (trenutneNarudzbe != null && trenutneNarudzbe?.Count != 0)
                return BadRequest("Trenutno ne mozete deaktivirati profil jer su Vase narudzbe u izradi");

          
            List<Narudzba> narudzbe = dbContext.Narudzba.Where(n => n.KorisnikId == korisnik.Id).ToList();
            List<NarudzbaStavka> stavkaNarudzbe = new List<NarudzbaStavka>();
            foreach (Narudzba narudzba in narudzbe)
            {
                stavkaNarudzbe.AddRange(dbContext.NarudzbaStavka.Where(sn => sn.NarudzbaId == narudzba.Id).ToList());
            }
            List<Rezervacija> rezervacije = dbContext.Rezervacija.Where(r => r.KorisnikId == korisnik.Id).ToList();
       
            List<AutentifikacijaToken> logovi = dbContext.AutentifikacijaToken.Where(at => at.KorisnickiNalogId == korisnik.Id).ToList();

            dbContext.NarudzbaStavka.RemoveRange(stavkaNarudzbe);
            dbContext.Narudzba.RemoveRange(narudzbe);
            dbContext.Rezervacija.RemoveRange(rezervacije);
            dbContext.AutentifikacijaToken.RemoveRange(logovi);
            dbContext.Korisnik.Remove(korisnik);
            dbContext.KorisnickiNalog.Remove(korisnik);
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public List<Korisnik> GetAll()
        {
            return dbContext.Korisnik.ToList();
        }

        [HttpPost]
        public ActionResult Update([FromBody] KorisnikUpdateVM korisnikUpdate)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("Nije logiran!");

            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;

            korisnik.Ime = korisnikUpdate.ime;
            korisnik.Prezime = korisnikUpdate.prezime;
            korisnik.KorisnickoIme = korisnikUpdate.korisnickoIme;
            korisnik.Lozinka = korisnikUpdate.lozinka;
            korisnik.Email = korisnikUpdate.email;
            korisnik.Adresa = korisnikUpdate.adresa;
            korisnik.BrojTelefona = korisnikUpdate.brojTelefona;
            korisnik.OpstinaId = korisnikUpdate.opstinaId;

            dbContext.SaveChanges();

            return Ok(korisnik);
        }

        [HttpPost("{id}")]
        public ActionResult AddKorisnikImage(int id, [FromForm] KorisnikImageAddVM x)
        {
            Korisnik korisnik = dbContext.Korisnik.FirstOrDefault(s => s.Id == id);

            if (korisnik == null)
                return BadRequest("neispravan korisnik ID");
            if (x.slikaKorisnika.Length > 300 * 1000)
                return BadRequest("max velicina fajla je 300 KB");

            string ekstenzija = Path.GetExtension(x.slikaKorisnika.FileName);

            var filename = $"{Guid.NewGuid()}{ekstenzija}";

            x.slikaKorisnika.CopyTo(new FileStream(Config.SlikeFolder + filename, FileMode.Create));
            korisnik.Slika = Config.SlikeURL + filename;
            dbContext.SaveChanges();

            return Ok(korisnik);

        }
        [HttpPost]
        public ActionResult UpdateKorisnikImage([FromForm] KorisnikImageAddVM x)
        {
            try
            {
                if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                    return BadRequest("nije logiran");
                Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;

                if (x.slikaKorisnika != null && korisnik != null)
                {
                    if (x.slikaKorisnika.Length > 300 * 1000)
                        return BadRequest("max velicina fajla je 300KB");

                    string ekstenzija = Path.GetExtension(x.slikaKorisnika.FileName);

                    var fileName = $"{Guid.NewGuid()}{ekstenzija}";

                    x.slikaKorisnika.CopyTo(new FileStream(Config.SlikeFolder + fileName, FileMode.Create));
                    korisnik.Slika = Config.SlikeURL + fileName;
                    dbContext.SaveChanges();
                }
                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException);
            }
        }
    }
}
