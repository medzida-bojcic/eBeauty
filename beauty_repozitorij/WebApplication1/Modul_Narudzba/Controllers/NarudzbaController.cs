using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.EntityModels;
using WebApplication1.Helper;
using WebApplication1.Helper.AutentifikacijaAutorizacija;
using WebApplication1.Modul_Narudzba.ViewModels;

namespace WebApplication1.Modul_Narudzba.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NarudzbaController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        public NarudzbaController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public ActionResult AddStavka([FromBody] NarudzbaAddStavkaVM stavkaAddVM)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");
            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;
            if (korisnik == null)
                return BadRequest("nepostojeci korisnik");

            Narudzba narudzba = _dbContext.Narudzba.Where(n => n.KorisnikId == korisnik.Id && n.Zakljucena == false).SingleOrDefault();
            if (narudzba == null)
            {
                narudzba = new Narudzba()
                {
                    DatumNarudzbe = DateTime.Now,
                    Zakljucena = false,
                    Korisnik = korisnik,
                    BrojStavki = 0,
                    Cijena = 0
                };
                _dbContext.Narudzba.Add(narudzba);
                _dbContext.SaveChanges();
            }
            Proizvod proizvodStavka = _dbContext.Proizvod.Find(stavkaAddVM.proizvodId);
            NarudzbaStavka stavkaNarudzbe = new NarudzbaStavka()
            {
                Kolicina = 1 ,
                ProizvodId = stavkaAddVM.proizvodId,
                NarudzbaId = narudzba.Id,
                Cijena = proizvodStavka.Cijena
            };
            _dbContext.NarudzbaStavka.Add(stavkaNarudzbe);
            narudzba.BrojStavki++;
            narudzba.Cijena += stavkaNarudzbe.Cijena;
            _dbContext.SaveChanges();

            return Ok(narudzba.BrojStavki);
        }
        [HttpGet]
        public ActionResult GetNarudzba()
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");
            int id = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik.Id;
            Narudzba narudzba = _dbContext.Narudzba.Where(n => n.KorisnikId == id && n.Zakljucena == false).FirstOrDefault();
            if (narudzba == null)
            {
                narudzba = new Narudzba()
                {
                    DatumNarudzbe = DateTime.Now,
                    Zakljucena = false,
                    KorisnikId = id,
                    BrojStavki = 0,
                    Cijena = 0,
                };
                _dbContext.Narudzba.Add(narudzba);
                _dbContext.SaveChanges();
            }
            NarudzbaGetNarudzbaVM getNarudzbaVM = new NarudzbaGetNarudzbaVM()
            {
                id = narudzba.Id,
                cijena = narudzba.Cijena,
                stavke = _dbContext.NarudzbaStavka.Where(sn => sn.NarudzbaId == narudzba.Id).Select(sn => new NarudzbaGetNarudzbaVM.Stavka()
                {
                    id = sn.Id,
                    naziv = sn.Proizvod.Naziv,
                    opis = sn.Proizvod.Opis,
                    cijena = sn.Proizvod.Cijena,
                    slika = sn.Proizvod.Slika,
                    kolicina = sn.Kolicina,
                    kategorijaId=sn.Proizvod.KategorijaId
                }).ToList(),
            };
            return Ok(getNarudzbaVM);
        }
        [HttpGet]
        public ActionResult GetBrojStavki()
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            int id = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik.Id;

            Korisnik korisnik = _dbContext.Korisnik.Find(id);
            if (korisnik == null) return Ok(0);

            Narudzba narudzba = _dbContext.Narudzba.Where(n => n.KorisnikId == id && n.Zakljucena == false).FirstOrDefault();
            if (narudzba == null) return Ok(0);

            return Ok(narudzba.BrojStavki);
        }
        [HttpGet("{id}")]
        public ActionResult UkloniStavku(int id)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            NarudzbaStavka stavkaNarudzbe = _dbContext.NarudzbaStavka.Where(sn => sn.Id == id).FirstOrDefault();

            Narudzba narudzba = _dbContext.Narudzba.Where(n => n.Id == stavkaNarudzbe.NarudzbaId).SingleOrDefault();
            if (narudzba == null)
                return BadRequest("Nepostojeca narudzba");

            _dbContext.NarudzbaStavka.Remove(stavkaNarudzbe);
            narudzba.Cijena -= stavkaNarudzbe.Cijena;
            narudzba.BrojStavki -= stavkaNarudzbe.Kolicina;
            _dbContext.SaveChanges();

            NarudzbaGetNarudzbaVM getNarudzbaVM = new NarudzbaGetNarudzbaVM()
            {
                id = narudzba.Id,
                cijena = narudzba.Cijena,
                stavke = _dbContext.NarudzbaStavka.Where(sn => sn.NarudzbaId == narudzba.Id).Select(sn => new NarudzbaGetNarudzbaVM.Stavka()
                {
                    id = sn.Id,
                    naziv = sn.Proizvod.Naziv,
                    opis = sn.Proizvod.Opis,
                    cijena = sn.Proizvod.Cijena,
                    slika = sn.Proizvod.Slika,
                    kolicina = sn.Kolicina,
                    kategorijaId=sn.Proizvod.KategorijaId
                }).ToList(),
            };

            return Ok(getNarudzbaVM);
        }
        [HttpPost]
        public ActionResult UpdateKolicina(NarudzbaUpdateKolicinaVM narudzbaUpdateKolicinaVM)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            NarudzbaStavka stavkaNarudzbe = _dbContext.NarudzbaStavka.Include(sn => sn.Proizvod)
                                            .Where(sn => sn.Id == narudzbaUpdateKolicinaVM.id).SingleOrDefault();
            if (stavkaNarudzbe == null)
                return BadRequest("Nepostojeca stavka narudzbe");


            Narudzba narudzba = _dbContext.Narudzba.Find(stavkaNarudzbe.NarudzbaId);
            narudzba.Cijena -= stavkaNarudzbe.Cijena;
            narudzba.BrojStavki -= stavkaNarudzbe.Kolicina;

            stavkaNarudzbe.Kolicina = narudzbaUpdateKolicinaVM.kolicina;

            stavkaNarudzbe.Cijena = stavkaNarudzbe.Proizvod.Cijena * narudzbaUpdateKolicinaVM.kolicina;
            

            narudzba.Cijena += stavkaNarudzbe.Cijena;
            narudzba.BrojStavki += stavkaNarudzbe.Kolicina;

            _dbContext.SaveChanges();

            var response = new
            {
                cijena = narudzba.Cijena,
                kolicina = narudzba.BrojStavki
            };

            return Ok(response);
        }

        [HttpGet("{pageNumber}")]
        public ActionResult GetAllPaged(int pageNumber)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;

            if (korisnik == null)
                return BadRequest("Nemate ovlasti za trazenu akciju!");

            var data = _dbContext.Narudzba.Where(n => n.KorisnikId == korisnik.Id && n.Zakljucena)
                                                            .Select(n => new NarudzbaGetAllPagedVM()
                                                            {
                                                                id = n.Id,
                                                                cijena = n.Cijena,
                                                                datumNarudzbe = n.DatumNarudzbe.ToString("dd/MM/yyyy hh:mm"),
                                                                statusNarudzbe = n.StatusNarudzbe.Naziv,
                                                                stavke = _dbContext.NarudzbaStavka.Where(sn => sn.NarudzbaId == n.Id).Select(sn => new NarudzbaGetAllPagedVM.Stavka()
                                                                {
                                                                    naziv = sn.Proizvod.Naziv,
                                                                    kolicina = sn.Kolicina
                                                                }).ToList()
                                                            }).AsQueryable();

            var mojeNarudzbe = PagedList<NarudzbaGetAllPagedVM>.Create(data, pageNumber, 6);

            return Ok(mojeNarudzbe);
        }



        [HttpGet("{id}")]
        public ActionResult Zakljuci(int id)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;

            Narudzba narudzba = _dbContext.Narudzba.Where(n => n.KorisnikId == korisnik.Id && n.Zakljucena == false).SingleOrDefault();
            if (narudzba == null)
                return BadRequest("Ne postoji aktivna narudzba!");

            narudzba.Zakljucena = true;

            narudzba.StatusNarudzbeId = _dbContext.StatusNarudzbe.Where(s => s.Naziv == "Poslano").SingleOrDefault().Id;

            Uposlenik odabraniUposlenik = _dbContext.Uposlenik
                .Where(z => z.AktivneNarudzbe == _dbContext.Uposlenik.Min<Uposlenik>(w => w.AktivneNarudzbe)).FirstOrDefault();
            odabraniUposlenik.AktivneNarudzbe++;
            narudzba.Uposlenik = odabraniUposlenik;
            if (odabraniUposlenik == null)
                return BadRequest("Nemamo uposlenika!");
            _dbContext.SaveChanges();

            return Ok(narudzba.Cijena);
        }

        [HttpGet("{id}")]
        public ActionResult Naruci(int id)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;

            if (korisnik == null)
                return BadRequest("Nemate ovlasti za trazenu akciju!");

            Narudzba narudzba = _dbContext.Narudzba.Find(id);
            narudzba.StatusNarudzbeId = _dbContext.StatusNarudzbe.Where(s => s.Naziv == "Poslano").SingleOrDefault().Id;
            Uposlenik odabraniUposlenik = _dbContext.Uposlenik
                .Where(z => z.AktivneNarudzbe == _dbContext.Uposlenik.Min<Uposlenik>(w => w.AktivneNarudzbe)).FirstOrDefault();
            odabraniUposlenik.AktivneNarudzbe++;
            narudzba.Uposlenik = odabraniUposlenik;
            if (odabraniUposlenik == null)
                return BadRequest("Nemamo uposlenika!");

            _dbContext.SaveChanges();
            string statusNaziv = _dbContext.Narudzba.Include(n => n.StatusNarudzbe).Where(n => n.Id == id).SingleOrDefault().StatusNarudzbe.Naziv;
            var response = new
            {
                status = statusNaziv,
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult Delete(int id)
        {
            if (!HttpContext.GetLoginInfo().isPermisijaKorisnik)
                return BadRequest("nije logiran");

            Korisnik korisnik = HttpContext.GetLoginInfo().korisnickiNalog.Korisnik;

            if (korisnik == null)
                return BadRequest("Nemate ovlasti za trazenu akciju!");

            Narudzba narudzba = _dbContext.Narudzba.Find(id);

            List<NarudzbaStavka> stavke = _dbContext.NarudzbaStavka.Where(sn => sn.NarudzbaId == id).ToList();

            _dbContext.RemoveRange(stavke);
            _dbContext.Narudzba.Remove(narudzba);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
