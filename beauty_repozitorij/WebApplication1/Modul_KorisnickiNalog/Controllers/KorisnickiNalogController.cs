using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.EntityModels;
using WebApplication1.Helper.AutentifikacijaAutorizacija;
using WebApplication1.Modul_KorisnickiNalog.ViewModels;
using WebApplication1.Modul_Korisnik.ViewModels;

namespace WebApplication1.Modul_KorisnickiNalog
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class KorisnickiNalogController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public KorisnickiNalogController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Get()
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("nije logiran");

            KorisnikGetVM korisnikGetVM = new KorisnikGetVM();
            KorisnickiNalog defaultniNalog = new KorisnickiNalog();

            if (HttpContext.GetLoginInfo().isPermisijaKorisnik)
            {
                Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;
                if (korisnik == null)
                    return BadRequest("Nepostojeci korisnik");

                defaultniNalog = korisnik;
                korisnikGetVM.brojTelefona = korisnik.BrojTelefona;
                korisnikGetVM.adresaStanovanja = korisnik.Adresa;
                korisnikGetVM.opstinaId = korisnik.OpstinaId;
            }
           /*else if (HttpContext.GetLoginInfo().isPermisijaAdmin)
            {
                Administrator admin = HttpContext.GetLoginInfo().korisnickiNalog.isAdmin;
                if (admin == null)
                    return BadRequest("Nepostojeci administrator");
                defaultniNalog = admin;
            }
            else if (HttpContext.GetLoginInfo().isPermisijaUposlenik)
            {
                Uposlenik uposlenik = HttpContext.GetLoginInfo().korisnickiNalog.isUposlenik;
                if (uposlenik == null)
                    return BadRequest("Nepostojeci zaposlenik");
                defaultniNalog = uposlenik;
                korisnikGetVM.slika = uposlenik.Slika;
            }*/
            

            korisnikGetVM.ime = defaultniNalog.Ime;
            korisnikGetVM.prezime = defaultniNalog.Prezime;
            korisnikGetVM.email = defaultniNalog.Email;
            korisnikGetVM.korisnickoIme = defaultniNalog.KorisnickoIme;
            korisnikGetVM.lozinka = defaultniNalog.Lozinka;

            return Ok(korisnikGetVM);
        }
        [HttpPost]
        public IActionResult UpdateAdmin([FromBody] AdminUpdateVM adminUpdateVM)
        {


            if (!HttpContext.GetLoginInfo().isPermisijaAdmin)
                return BadRequest("Nije logiran");

            Administrator admin = HttpContext.GetLoginInfo().korisnickiNalog.Administrator;
            if (admin == null)
                return BadRequest("Nepostojeci administrator");

            admin.Ime = adminUpdateVM.ime;
            admin.Prezime = adminUpdateVM.prezime;
            admin.Email = adminUpdateVM.email;
            admin.KorisnickoIme = adminUpdateVM.korisnickoIme;
            admin.Lozinka = adminUpdateVM.lozinka;
            admin.DatumKreiranja = DateTime.Now;

            _dbContext.SaveChanges();
            return Ok(admin);

        }
    }
}
